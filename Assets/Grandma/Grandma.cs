using UnityEngine;
using UnityEngine.AI;

public class Grandma : MonoBehaviour
{
    [Header("Tuning")]
    public float wanderSpeed = 2;
    public float wanderAngularSpeed = 100;
    public float wanderAcceleration = 5;

    public float chaseSpeedMultiplier = 5;

    [Header("Other")]
    public NavMeshAgent agent;
    public Transform debugTarget;

    public static Grandma instance;

    /// <summary>
    /// Grandma AI's state machine
    /// </summary>
    public enum State
    {
        wandering,
        chasingNoise,
        examiningNoise
    }
    public State state = State.wandering;
    State oldState = State.wandering;   // (For seeing when state changed)

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (instance != null)
            Debug.LogError("2 grandma instances in the scene! Should only be 1! The second instance is " + this);
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    float wanderTimer = 0;
    float noiseChaseTimer = 0;
    float examineNoiseTimer = 0;

    // Update is called once per frame
    void Update()
    {
        State tempOldState = state;

        if (state is State.wandering)
        {
            // wander around randomly (but with predictable timing)
            if (oldState != State.wandering)
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
        } else if (state is State.chasingNoise)
        {
            if (targetPosition != Vector3.zero)
            {
                agent.SetDestination(targetPosition);
            } else
            {
                Debug.LogWarning("targetPosition not set! Can't chase noise!");
                state = State.wandering;
            }

            agent.acceleration = wanderAcceleration * chaseSpeedMultiplier;
            agent.speed = wanderSpeed * chaseSpeedMultiplier;
            agent.angularSpeed = wanderAngularSpeed * chaseSpeedMultiplier;

            if (Vector3.Distance(targetPosition, transform.position) < 2.5f)
            {
                state = State.examiningNoise;
                examineNoiseTimer = 3;
            }

            noiseChaseTimer -= Time.deltaTime;
            if (noiseChaseTimer <= 0)   // So she doesn't get stuck chasing one out of bounds noise forever
            {
                // TODO an animation for forgetting what she was doing..? Eh
                state = State.wandering;
            }
        } else if (state is State.examiningNoise)
        {
            // TODO an examining animation

            examineNoiseTimer -= Time.deltaTime;
            if (examineNoiseTimer <= 0)
                state = State.wandering;
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

    public Vector3 targetPosition;

    /// <summary>
    /// Grandma got alerted to a noise! She'll chase it!
    /// </summary>
    /// <param name="noise"></param>
    public void noiseAlert(Noise noise)
    {
        targetPosition = noise.noiseSourcePosition;
        print("noiseAlert " + targetPosition);
        // Todo play a grandma alert animation/screech etc

        noiseChaseTimer = 10f;
        state = State.chasingNoise;
    }
}
