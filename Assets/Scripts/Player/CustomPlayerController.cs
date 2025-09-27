using EdmontonJam.Manager;
using EdmontonJam.Prop;
using Sketch.FPS;
using Sketch.Translation;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace EdmontonJam.Player
{
    public class CustomPlayerController : PlayerController
    {
        [SerializeField]
        private Transform _hands;

        [SerializeField]
        private TMP_Text _lockpickText;

        public ObjectiveProp HoldedObject { set; get; }

        public SpawnPoint AttachedSpawn { set; get; }

        private int _lockpickCount;

        protected override void Awake()
        {
            base.Awake();

            UpdateUI();
        }

        public void GrabLockpick()
        {
            _lockpickCount++;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _lockpickText.text = _lockpickCount.ToString();
        }

        public void HoldObject(ObjectiveProp p)
        {
            Assert.IsNull(HoldedObject);

            HoldedObject = p;
            p.transform.parent = _hands.transform;
            p.transform.localPosition = Vector3.zero;

            RemoveInteraction(p);
            p.enabled = false;

            if (!GameManager.Instance.IsChasing) GameManager.Instance.IsChasing = true;
        }

        public override string GetInteractionText(string interactionVerb)
        {
            return Translate.Instance.Tr("interactionText", _pInput.currentControlScheme == "Keyboard&Mouse" ? "E" : Translate.Instance.Tr("southButton"), Translate.Instance.Tr(interactionVerb));
        }

        public override string GetDenyText(string denySentence)
        {
            return Translate.Instance.Tr(denySentence);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IPickable>(out var p))
            {
                p.Pick(this);
            }
        }
    }
}