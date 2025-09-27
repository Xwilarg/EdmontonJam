using EdmontonJam.Noise;
using EdmontonJam.SO;
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

        public void SpawnNoise(Vector3 startPos, NoiseInfo nInfo)
        {
            startPos.y = 1f;

            var chaserNoise = Instantiate(_noisePrefab, startPos, Quaternion.identity).GetComponent<Onomatopiea>();
            chaserNoise.grandmaChaser = true;
            chaserNoise.noiseSourcePosition = startPos;
            chaserNoise.NoiseInfo = nInfo;

            Instantiate(_noisePrefab, startPos, Quaternion.identity).GetComponent<Onomatopiea>().NoiseInfo = nInfo;
        }
    }
}