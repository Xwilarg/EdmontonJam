using System.Collections.Generic;
using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    public GameObject noisePrefab;

    [Tooltip("Press to send a test noise out")]
    public bool testNoise;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


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
        Noise chaserNoise = Instantiate(noisePrefab, transform.position + transform.up, Quaternion.identity).GetComponent<Noise>();
        chaserNoise.grandmaChaser = true;
        chaserNoise.noiseSourcePosition = transform.position;

        Noise popupNoise = Instantiate(noisePrefab, transform.position + transform.up, Quaternion.identity).GetComponent<Noise>();
    }
}
