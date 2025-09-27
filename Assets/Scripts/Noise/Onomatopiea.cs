using EdmontonJam.Grandma;
using UnityEngine;
using UnityEngine.AI;

namespace EdmontonJam.Noise
{
    /// <summary>
    /// Onomatopieas that pop up when noises are made, and also fly over to alert grandma to chase them
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Onomatopiea : MonoBehaviour
    {

        [Header("Animation")]

        [Tooltip("The noise object will sproing this many times a second")]
        public float bounceSpeed = 1.5f;

        [Tooltip("How enthusiastic to bounce")]
        [Range(0, 1)]
        public float bounceAmount = .3f;

        [Tooltip("The visible object that gets bounced around. (The script tries to find it automatically if left blank)")]
        public Transform noiseRenderObject;     // Had to separate it because scaling the nav agent object screws it up
        Material material;  // material instance, to adjust opacity

        [Header("Chase Grandma")]
        [Tooltip("This noise will chase and alert grandma. If false, it just pops up in place to show the source of the noise")]
        public bool grandmaChaser = true;

        public float hoverHeight = 2f;

        public float chaseSpeed = 20f;
        public float turnSpeed = 1000f;
        public float chaseAcceleration = 60;

        /// <summary>
        /// Where grandma will run to
        /// </summary>
        public Vector3 noiseSourcePosition;



        Vector3 startingScale;  // local
        Vector3 startingPosition;   // global
        NavMeshAgent agent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startingScale = transform.localScale;
            startingPosition = transform.position;

            agent = GetComponent<NavMeshAgent>();


            if (noiseRenderObject == null)
            {
                MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                noiseRenderObject = renderers[0].transform;
            }
            // duplicate material into instance, to modify its alpha
            MeshRenderer renderer = noiseRenderObject.GetComponent<MeshRenderer>();
            Material baseMaterial = renderer.material;
            material = new Material(baseMaterial);
            renderer.material = material;

        }


        float animate = 0;  // bounce timer

        float caughtGrandmaTimer = 0;
        float caughtGrandmaTimerMax = 2;

        // Update is called once per frame
        void Update()
        {
            agent.baseOffset = hoverHeight;
            //Physics.IgnoreCollision;
            //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            // Animate
            if (Camera.main != null)
                noiseRenderObject.LookAt(Camera.main.transform.position);    // point at cam

            // bounce
            {
                animate += Time.deltaTime * bounceSpeed;

                Vector2 bounce = new Vector2(Mathf.Cos(animate * Mathf.PI * 2), Mathf.Sin(animate * Mathf.PI * 2));
                bounce = new Vector2(bounce.x * bounceAmount, bounce.y * bounceAmount) + new Vector2((1 - bounceAmount), (1 - bounceAmount));

                noiseRenderObject.localScale = Vector3.Scale(startingScale, new Vector3(bounce.x, bounce.y, 1));

                if (animate >= 1)
                    animate = 0;
            }


            // chasing
            if (grandmaChaser)
            {
                if (GrandmaController.instance == null)
                    Debug.LogWarning("Grandma not spawned in scene!");
                else  // handle chase
                {
                    GrandmaController gran = GrandmaController.instance;
                    agent.SetDestination(gran.transform.position);
                    agent.speed = chaseSpeed;
                    agent.angularSpeed = turnSpeed;
                    agent.acceleration = chaseAcceleration;

                    if (Vector3.Distance(transform.position, gran.transform.position) < 2.5f && caughtGrandmaTimer == 0)
                    {
                        caughtGrandmaTimer = .1f;

                        gran.noiseAlert(this);
                    }

                    if (caughtGrandmaTimer > 0)
                    {
                        //print("caught " + caughtGrandmaTimer);
                        //float scale = Mathf.Cos(caughtGrandmaTimer * Mathf.PI * 2);
                        float scale = caughtGrandmaTimer;
                        noiseRenderObject.localScale = Vector3.Scale(noiseRenderObject.localScale, Vector3.one * scale + Vector3.one);

                        material.SetFloat("_Alpha", Mathf.Clamp01(caughtGrandmaTimerMax - caughtGrandmaTimer));

                        caughtGrandmaTimer += Time.deltaTime * 6f;
                        if (caughtGrandmaTimer > caughtGrandmaTimerMax)
                        {
                            Destroy(gameObject);  // TODO would object pool these in a serious project
                        }
                    }
                }
            }
            else
            {
                // not grandmaChaser
                agent.enabled = false;


                // animate
                float hoverAni = Mathf.Log10(hoverTimer * 20 + .3f);
                //print("hoverAni " + hoverAni);
                transform.position = startingPosition + new Vector3(0, hoverAni * 2 - 1, 0);
                noiseRenderObject.transform.localScale = noiseRenderObject.localScale * ((hoverAni * 2) + 0);

                material.SetFloat("_Alpha", Mathf.Clamp01(hoverTimerMax - hoverTimer));
                //print("material alpha " + material.GetFloat("_Alpha"));

                hoverTimer += Time.deltaTime;
                if (hoverTimer > hoverTimerMax)
                {
                    Destroy(gameObject);
                }
            }
        }
        float hoverTimer = 0;
        float hoverTimerMax = 2.5f;

    }
}
