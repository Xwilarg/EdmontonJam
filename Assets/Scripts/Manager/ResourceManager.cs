using EdmontonJam.SO;
using Sketch.Translation;
using Sketch.VN;
using System.Collections;
using TMPro;
using UnityEngine;

namespace EdmontonJam.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        [SerializeField]
        private GameInfo _gameInfo;

        [SerializeField]
        private TextDisplay _warning;

        [SerializeField]
        private GameObject _victoryText;

        public GameInfo GameInfo => _gameInfo;

        private bool _isWaitingDeletion;

        private void Awake()
        {
            Instance = this;
            _warning.gameObject.SetActive(false);
            _warning.DisplaySpeedRef = .1f;
        }

        private void Update()
        {
            if (!_isWaitingDeletion && _warning.gameObject.activeInHierarchy && _warning.IsDisplayDone)
            {
                _isWaitingDeletion = true;
                StartCoroutine(WaitAndRemoveTextCoroutine());
            }
        }

        private IEnumerator WaitAndRemoveTextCoroutine()
        {
            yield return new WaitForSeconds(2f);
            _warning.gameObject.SetActive(false);
        }

        public void SetWarningText()
        {
            _warning.gameObject.SetActive(true);
            _warning.ToDisplay = Translate.Instance.Tr("grandmaWarning");
        }

        public void ShowVictoryWarning()
        {
            _warning.gameObject.SetActive(true);
            _warning.ToDisplay = Translate.Instance.Tr("grandmaWarning");
            _isWaitingDeletion = false;
        }
    }
}