using EdmontonJam.SO;
using Sketch.Translation;
using Sketch.VN;
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

        public GameInfo GameInfo => _gameInfo;

        private void Awake()
        {
            Instance = this;
            _warning.gameObject.SetActive(false);
        }

        public void SetWarningText()
        {
            _warning.gameObject.SetActive(true);
            _warning.ToDisplay = Translate.Instance.Tr("grandmaWarning");
        }
    }
}