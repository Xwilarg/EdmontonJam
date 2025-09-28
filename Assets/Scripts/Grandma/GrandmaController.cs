using EdmontonJam.Manager;
using EdmontonJam.Noise;
using EdmontonJam.Player;
using EdmontonJam.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace EdmontonJam.Grandma
{
    public class GrandmaController : MonoBehaviour
    {
        private class PlayerChaseInfo
        {
            public CustomPlayerController Player;
            public GameObject PlayerGO;
            public float IgnoreTimer;
        }

        [SerializeField]
        private Transform _hands;

        [Header("Tuning")]
        public float wanderSpeed = 2;
        public float wanderAngularSpeed = 100;
        public float wanderAcceleration = 5;

        public float chaseSpeedMultiplier = 5;

        [Header("Other")]
        public NavMeshAgent agent;
        public Transform debugTarget;

        public static GrandmaController instance;

        // Info of the noise we are currently chasing
        private Onomatopiea _ono;
        private NoiseInfo _noiseInfo;

        private readonly List<PlayerChaseInfo> _players = new();

        public bool IsPlayerHoldingItem => _players.Any(x => x.Player.HoldedObject != null);

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(10, 10, 500, 500), $"State: {State}");
#endif
        }

        /// <summary>
        /// Grandma AI's state machine
        /// </summary>
        public enum BehaviorsState
        {
            wandering,
            chasingNoise,
            examiningNoise,
            carryPlayerRoom,
            chasingPlayer
        }
        private BehaviorsState _state = BehaviorsState.wandering;
        public BehaviorsState State
        {
            set
            {
                if (value != BehaviorsState.chasingNoise)
                {
                    _noiseInfo = null;
                }

                if (value == BehaviorsState.wandering)
                {
                    agent.acceleration = wanderAcceleration;
                    agent.speed = wanderSpeed;
                    agent.angularSpeed = wanderAngularSpeed;
                }
                else if (value == BehaviorsState.chasingNoise || value == BehaviorsState.carryPlayerRoom || value == BehaviorsState.chasingPlayer)
                {
                    agent.acceleration = wanderAcceleration * chaseSpeedMultiplier;
                    agent.speed = wanderSpeed * chaseSpeedMultiplier;
                    agent.angularSpeed = wanderAngularSpeed * chaseSpeedMultiplier;
                }
                
                _state = value;
            }
            get => _state;
        }
        BehaviorsState oldState = BehaviorsState.wandering;   // (For seeing when state changed)

        private CustomPlayerController _carriedPlayer, _chasedPlayer;
        public bool IsCarryingSomeone => _carriedPlayer != null;

        public void Register(CustomPlayerController player)
        {
            _players.Add(new()
            {
                Player = player,
                PlayerGO = player.gameObject
            });
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if (instance != null)
                Debug.LogError("2 grandma instances in the scene! Should only be 1! The second instance is " + this);
            instance = this;
        }

        float wanderTimer = 0;
        float noiseChaseTimer = 0;
        float examineNoiseTimer = 0;
        float chasingTimer = 0;
        private const float ChasingTimerRef = 1f;

        private void OnDrawGizmos()
        {
            foreach (var player in _players)
            {
                if (player.IgnoreTimer <= 0f)
                {
                    if (Physics.Linecast(transform.position, player.PlayerGO.transform.position, LayerMask.GetMask("Map", "Prop")))
                    {
                        Physics.Raycast(transform.position, player.PlayerGO.transform.position - transform.position, out var hit, float.MaxValue, LayerMask.GetMask("Map", "Prop"));
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(transform.position, hit.point);
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(transform.position, player.PlayerGO.transform.position);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.IsChasing && State != BehaviorsState.carryPlayerRoom && State != BehaviorsState.chasingPlayer) // No need to look for player if we are already carrying one
            {
                _players.RemoveAll(x => x.PlayerGO == null); // TODO

                foreach (var player in _players)
                {
                    if (player.IgnoreTimer > 0f) player.IgnoreTimer -= Time.deltaTime;
                    else // Try to look at player
                    {
                        if (!Physics.Linecast(transform.position, player.PlayerGO.transform.position, LayerMask.GetMask("Map", "Prop"))) // We saw someone!
                        {
                            _chasedPlayer = player.Player;
                            State = BehaviorsState.chasingPlayer;
                            chasingTimer = ChasingTimerRef;

                            agent.SetDestination(player.Player.transform.position);
                            break;
                        }
                    }
                }
            }

            BehaviorsState tempOldState = State;

            if (State is BehaviorsState.wandering)
            {
                // wander around randomly (but with predictable timing)
                if (oldState != BehaviorsState.wandering)
                    wanderTimer = 1000; // to reset target

                wanderTimer += Time.deltaTime;
                if (wanderTimer > 5)    // reset target
                {
                    wanderTimer = 0;
                    Vector3 position = getRandomNavmeshPoint();
                    agent.SetDestination(position);
                    //print("position " + position);

                }
            }
            else if (State is BehaviorsState.chasingNoise)
            {
                if (targetPosition != null)
                {
                    agent.SetDestination(targetPosition.Value);
                }
                else
                {
                    Debug.LogWarning("targetPosition not set! Can't chase noise!");
                    State = BehaviorsState.wandering;
                    return;
                }

                if (Vector3.Distance(targetPosition.Value, transform.position) < 2.5f)
                {
                    State = BehaviorsState.examiningNoise;
                    examineNoiseTimer = 3;
                    _ono.Source.StunNoiseReporting(5f);
                }

                noiseChaseTimer -= Time.deltaTime;
                if (noiseChaseTimer <= 0)   // So she doesn't get stuck chasing one out of bounds noise forever
                {
                    // TODO an animation for forgetting what she was doing..? Eh
                    State = BehaviorsState.wandering;
                }
            }
            else if (State is BehaviorsState.carryPlayerRoom)
            {
                if (Vector3.Distance(_carriedPlayer.AttachedSpawn.transform.position, transform.position) < 2.5f)
                {
                    State = BehaviorsState.wandering;

                    _carriedPlayer.Drop();
                    _carriedPlayer = null;
                }
            }
            else if (State is BehaviorsState.examiningNoise)
            {
                // TODO an examining animation

                examineNoiseTimer -= Time.deltaTime;
                if (examineNoiseTimer <= 0)
                    State = BehaviorsState.wandering;
            }
            else if (State is BehaviorsState.chasingPlayer)
            {
                chasingTimer -= Time.deltaTime;

                if (chasingTimer <= 0f)
                {
                    if (!Physics.Linecast(transform.position, _chasedPlayer.transform.position, LayerMask.GetMask("Map", "Prop")))
                    {
                        // We still see the player
                        chasingTimer = ChasingTimerRef;

                        agent.SetDestination(_chasedPlayer.transform.position);
                    }
                    else
                    {
                        // LoS lost
                        State = BehaviorsState.wandering;
                        _players.First(x => x.PlayerGO.GetInstanceID() == _chasedPlayer.gameObject.GetInstanceID()).IgnoreTimer = 5f;
                        _chasedPlayer = null;
                    }
                }
            }

            oldState = tempOldState;
        }

        public void Carry(CustomPlayerController pc)
        {
            Assert.IsFalse(IsCarryingSomeone);

            _carriedPlayer = pc;
            _carriedPlayer.transform.parent = _hands;
            _carriedPlayer.transform.localPosition = Vector3.zero;

            State = BehaviorsState.carryPlayerRoom;

            agent.SetDestination(pc.AttachedSpawn.transform.position);
        }

        public Vector3 getRandomNavmeshPoint()
        {
            // Alt idea:
            //Vector3 position = Random.insideUnitSphere * 100;
            //position = NavMesh.SamplePosition()

            NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
            int indice = (int)((triangles.vertices.Length - 1) * Random.value);
            return triangles.vertices[indice];
        }

        public Vector3? targetPosition;

        /// <summary>
        /// Grandma got alerted to a noise! She'll chase it!
        /// </summary>
        /// <param name="noise"></param>
        public void noiseAlert(Onomatopiea noise)
        {
            if (!GameManager.Instance.IsChasing) return; // We are not in hunting phase yet

            if (State == BehaviorsState.carryPlayerRoom || State == BehaviorsState.chasingPlayer) // Carrying or chasing a player is more important than a noise!
            {
                return;
            }

            if (State == BehaviorsState.chasingNoise && noise.NoiseInfo.NoiseForce < _noiseInfo.NoiseForce) // We received a noise but we are already chasing a more important one
            {
                return;
            }

            _ono = noise;
            _noiseInfo = noise.NoiseInfo;
            targetPosition = noise.noiseSourcePosition;
            print("noiseAlert " + targetPosition);
            // Todo play a grandma alert animation/screech etc

            noiseChaseTimer = 10f;
            State = BehaviorsState.chasingNoise;
        }
    }
}

