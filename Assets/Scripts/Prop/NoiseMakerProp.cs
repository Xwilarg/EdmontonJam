using EdmontonJam.Manager;
using EdmontonJam.Noise;
using EdmontonJam.Player;
using EdmontonJam.SO;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class NoiseMakerProp : MonoBehaviour, IPickable, INoiseMaker
    {
        [SerializeField]
        private NoiseInfo _info;

        private bool _isOn;
        private float _timer;

        public void Pick(CustomPlayerController cpc)
        {
            _isOn = true;
            _timer = 1f;
        }

        public void StunNoiseReporting(float time)
        {
            _isOn = false;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = 1f;
                NoiseManager.Instance.SpawnNoise(transform.position, _info, this);
            }
        }
    }
}