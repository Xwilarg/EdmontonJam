using EdmontonJam.Manager;
using EdmontonJam.Noise;
using EdmontonJam.SO;
using UnityEngine;
using UnityEngine.AI;

namespace EdmontonJam.Grandma
{
    public class GrandmaController : MonoBehaviour
    {
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
        private NoiseInfo _noiseInfo;

        /// <summary>
        /// Grandma AI's state machine
        /// </summary>
        public enum BehaviorsState
        {
            wandering,
            chasingNoise,
            examiningNoise
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
                _state = value;
            }
            get => _state;
        }
        BehaviorsState oldState = BehaviorsState.wandering;   // (For seeing when state changed)

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

        // Update is called once per frame
        void Update()
        {
            BehaviorsState tempOldState = State;

            if (State is BehaviorsState.wandering)
            {
                // wander around randomly (but with predictable timing)
                if (oldState != BehaviorsState.wandering)
                    wanderTimer = 1000; // to reset target

                agent.acceleration = wanderAcceleration;
                agent.speed = wanderSpeed;
                agent.angularSpeed = wanderAngularSpeed;


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

                agent.acceleration = wanderAcceleration * chaseSpeedMultiplier;
                agent.speed = wanderSpeed * chaseSpeedMultiplier;
                agent.angularSpeed = wanderAngularSpeed * chaseSpeedMultiplier;

                if (Vector3.Distance(targetPosition.Value, transform.position) < 2.5f)
                {
                    State = BehaviorsState.examiningNoise;
                    examineNoiseTimer = 3;
                }

                noiseChaseTimer -= Time.deltaTime;
                if (noiseChaseTimer <= 0)   // So she doesn't get stuck chasing one out of bounds noise forever
                {
                    // TODO an animation for forgetting what she was doing..? Eh
                    State = BehaviorsState.wandering;
                }
            }
            else if (State is BehaviorsState.examiningNoise)
            {
                // TODO an examining animation

                examineNoiseTimer -= Time.deltaTime;
                if (examineNoiseTimer <= 0)
                    State = BehaviorsState.wandering;
            }

            oldState = tempOldState;
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

            if (State == BehaviorsState.chasingNoise && noise.NoiseInfo.NoiseForce < _noiseInfo.NoiseForce) // We received a noise but we are already chasing a more important one
            {
                return;
            }

            _noiseInfo = noise.NoiseInfo;
            targetPosition = noise.noiseSourcePosition;
            print("noiseAlert " + targetPosition);
            // Todo play a grandma alert animation/screech etc

            noiseChaseTimer = 10f;
            State = BehaviorsState.chasingNoise;
        }
    }
}

