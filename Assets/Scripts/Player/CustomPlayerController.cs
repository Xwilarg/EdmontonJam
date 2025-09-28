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

        [SerializeField]
        private GameObject _knife;

        [SerializeField]
        private GameObject _ghostPrefab;

        [SerializeField]
        private GameObject _modelContainer;

        private CharacterController _cc;

        private GameObject _ghost;

        public ObjectiveProp HoldedObject { set; get; }
        private GameObject _holdedChild;

        public SpawnPoint AttachedSpawn { set; get; }

        private bool _hasKnife;
        public bool HasKnife
        {
            set
            {
                _hasKnife = value;
                _knife.SetActive(value);
            }
            get => _hasKnife;
        }

        private int _lockpickCount;

        private bool _isActive = true;
        public override bool IsActive => _isActive;

        protected override void Awake()
        {
            base.Awake();

            _cc = GetComponent<CharacterController>();

            UpdateUI();

            _knife.SetActive(false);

            _ghost = Instantiate(_ghostPrefab);
            LevelManager.Instance.MoveToMinimapPosition(transform.position, _ghost);
        }

        protected override void Update()
        {
            base.Update();

            LevelManager.Instance.MoveToMinimapPosition(transform.position, _ghost);
        }

        public void SpawnModel(GameObject gameObject)
        {
            var go = Instantiate(gameObject, _modelContainer.transform);
            go.transform.localPosition = Vector3.zero;
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
            _holdedChild = p.transform.GetChild(0).gameObject;
            _holdedChild.transform.parent = _hands.transform;
            _holdedChild.transform.localPosition = Vector3.zero;

            if (!GameManager.Instance.IsChasing) GameManager.Instance.IsChasing = true;
        }

        public void Drop()
        {
            _cc.enabled = true;
            _isActive = true;
            transform.parent = null;
        }

        public void ConsumeItem()
        {
            Destroy(_holdedChild);
            HoldedObject = null;
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
                if (HasKnife)
                {
                    ResourceManager.Instance.ShowVictory();
                    Destroy(GrandmaController.instance.gameObject);
                }
                else
                {
                    var grandma = other.GetComponent<GrandmaController>();
                    if (grandma.IsCarryingSomeone || !grandma.CanCarry(this)) return;

                    if (HoldedObject != null)
                    {
                        _holdedChild.transform.parent = HoldedObject.transform;
                        _holdedChild.transform.localPosition = Vector3.zero;

                        HoldedObject.WasTaken = false;

                        HoldedObject = null;
                    }

                    _cc.enabled = false;
                    _isActive = false;
                    transform.rotation = other.transform.rotation;
                    grandma.Carry(this);
                } 
            }
        }
    }
}