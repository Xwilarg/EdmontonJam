using EdmontonJam.Manager;
using EdmontonJam.SO;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private NoiseInfo _noiseInfo;

        private Rigidbody _rb;
        private float _noiseTimer;

        private void Awake()
        {
            _rb = transform.GetChild(0).GetComponentInChildren<Rigidbody>();
        }

        private void Update()
        {
            if (_noiseTimer > 0f)
            {
                _noiseTimer -= Time.deltaTime;
            }
            else if (_rb.angularVelocity.magnitude > 0f)
            {
                _noiseTimer = .5f;

                NoiseManager.Instance.SpawnNoise(transform.position, _noiseInfo);
            }
        }
    }
}