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

        [SerializeField]
        private GameObject _ghostPrefab;

        private bool _isOn;
        private float _timer;

        private void Start()
        {
            var go = Instantiate(_ghostPrefab);
            LevelManager.Instance.MoveToMinimapPosition(transform.position, go);
        }

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
            if (!_isOn) return;

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timer = 1f;
                NoiseManager.Instance.SpawnNoise(transform.position, _info, this);
            }
        }
    }
}