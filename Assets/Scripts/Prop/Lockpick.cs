using EdmontonJam.Manager;
using EdmontonJam.Player;
using System.Collections;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class Lockpick : MonoBehaviour, IPickable
    {
        [SerializeField]
        private GameObject _model;

        [SerializeField]
        private float _rotSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * _rotSpeed);
        }

        public void Pick(CustomPlayerController cpc)
        {
            cpc.GrabLockpick();

            gameObject.layer = LayerMask.NameToLayer("Default");
            _model.SetActive(false);

            StartCoroutine(ReloadCoroutine());
        }

        public IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(ResourceManager.Instance.GameInfo.LockpickReloadTime);

            gameObject.layer = LayerMask.NameToLayer("Prop");
            _model.SetActive(true);
        }
    }
}