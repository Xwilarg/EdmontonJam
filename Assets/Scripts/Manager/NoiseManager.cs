using EdmontonJam.Noise;
using EdmontonJam.SO;
using Unity.Properties;
using UnityEngine;

namespace EdmontonJam.Manager
{
    public class NoiseManager : MonoBehaviour
    {
        public static NoiseManager Instance { private set; get; }

        [SerializeField]
        private GameObject _noisePrefab;

        private void Awake()
        {
            Instance = this;
        }

        private void SetMaterial(GameObject go, Material mat)
        {
            go.GetComponentInChildren<Renderer>().material = mat;
        }

        public void SpawnNoise(Vector3 startPos, NoiseInfo nInfo, bool chase = true)
        {
            startPos.y = 1f;

            GameObject go;
            if (chase)
            {
                go = Instantiate(_noisePrefab, startPos, Quaternion.identity);
                var chaserNoise = go.GetComponent<Onomatopiea>();
                chaserNoise.GrandmaChaser = true;
                chaserNoise.noiseSourcePosition = startPos;
                chaserNoise.NoiseInfo = nInfo;
                SetMaterial(go, nInfo.Material);
                go.transform.localScale = Vector3.one * (Mathf.Clamp01(nInfo.NoiseForce / 5f) / 2 + .5f);
            }

            go = Instantiate(_noisePrefab, startPos, Quaternion.identity);
            go.GetComponent<Onomatopiea>().NoiseInfo = nInfo;
            SetMaterial(go, nInfo.Material);
            go.transform.localScale = Vector3.one * (Mathf.Clamp01(nInfo.NoiseForce / 5f) / 2 + .5f);
        }
    }
}