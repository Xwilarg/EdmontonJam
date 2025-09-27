using EdmontonJam.Grandma;
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

        private CharacterController _cc;

        public ObjectiveProp HoldedObject { set; get; }

        public SpawnPoint AttachedSpawn { set; get; }

        private int _lockpickCount;

        private bool _isActive = true;
        public override bool IsActive => _isActive;

        protected override void Awake()
        {
            base.Awake();

            _cc = GetComponent<CharacterController>();

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

            if (!GameManager.Instance.IsChasing) GameManager.Instance.IsChasing = true;
        }

        public void Drop()
        {
            _cc.enabled = true;
            _isActive = true;
            transform.parent = null;
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

            if (GameManager.Instance.IsChasing && other.CompareTag("Grandma"))
            {
                var grandma = other.GetComponent<GrandmaController>();
                if (grandma.IsCarryingSomeone) return;

                _cc.enabled = false;
                _isActive = false;
                transform.rotation = other.transform.rotation;
                grandma.Carry(this);
            }
        }
    }
}