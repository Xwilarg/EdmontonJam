using EdmontonJam.Manager;
using EdmontonJam.Noise;
using EdmontonJam.SO;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class Door : MonoBehaviour, INoiseMaker
    {
        [SerializeField]
        private NoiseInfo _noiseInfo;

        private Rigidbody _rb;
        private float _noiseTimer;

        private float _disllowEmitTimer;
        public void StunNoiseReporting(float time)
        {
            _disllowEmitTimer = time;
        }

        private void Awake()
        {
            _rb = transform.GetChild(0).GetComponentInChildren<Rigidbody>();
        }

        private void Update()
        {
            if (_disllowEmitTimer > 0f)
            {
                _disllowEmitTimer -= Time.deltaTime;
            }

            if (_noiseTimer > 0f)
            {
                _noiseTimer -= Time.deltaTime;
            }
            else if (_rb.angularVelocity.magnitude > ResourceManager.Instance.GameInfo.MinDoorMagnitudeForNoise && _disllowEmitTimer <= 0f)
            {
                _noiseTimer = .5f;

                NoiseManager.Instance.SpawnNoise(transform.position, _noiseInfo);
            }
        }
    }
}