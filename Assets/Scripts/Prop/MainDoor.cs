using EdmontonJam.Grandma;
using EdmontonJam.Manager;
using EdmontonJam.Noise;
using EdmontonJam.Player;
using EdmontonJam.SO;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class MainDoor : MonoBehaviour, INoiseMaker, IPickable
    {
        [SerializeField]
        private GameObject _noiseEmitter;

        [SerializeField]
        private NoiseInfo _info;

        [SerializeField]
        private Rigidbody[] _rbs;

        private float _timer;

        public void Pick(CustomPlayerController cpc)
        {
            if (cpc.HoldedObject != null)
            {
                cpc.ConsumeItem();
                GameManager.Instance.ItemLeft--;

                if (GameManager.Instance.ItemLeft == 0)
                {
                    foreach (var rb in _rbs)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
        }

        public void StunNoiseReporting(float time)
        {
            _timer += time;
        }

        private void Update()
        {
            if (!GrandmaController.instance.IsPlayerHoldingItem) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                NoiseManager.Instance.SpawnNoise(_noiseEmitter.transform.position, _info, this);
                _timer = 3f;
            }
        }
    }
}