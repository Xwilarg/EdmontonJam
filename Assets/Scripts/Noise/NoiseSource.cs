using UnityEngine;

namespace EdmontonJam.Noise
{
    public class NoiseSource : MonoBehaviour
    {
        public GameObject noisePrefab;

        [Tooltip("Press to send a test noise out")]
        public bool testNoise;


        //public Transform noiseObject;

        //public Dictionary<bool, bool> test;

        // Update is called once per frame
        void Update()
        {
            if (testNoise)
            {
                testNoise = false;
                spawnNoise();
            }
        }

        public void spawnNoise()
        {
            //Transform noiseObject = Instantiate(noisePrefab, transform.position + transform.up, Quaternion.identity).transform;
            Onomatopiea chaserNoise = Instantiate(noisePrefab, transform.position + transform.up, Quaternion.identity).GetComponent<Onomatopiea>();
            chaserNoise.grandmaChaser = true;
            chaserNoise.noiseSourcePosition = transform.position;

            Onomatopiea popupNoise = Instantiate(noisePrefab, transform.position + transform.up, Quaternion.identity).GetComponent<Onomatopiea>();
        }
    }
}

