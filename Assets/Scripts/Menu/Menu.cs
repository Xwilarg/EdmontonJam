using EdmontonJam.Grandma;
using Sketch.Translation;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdmontonJam.Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private MenuAgentController _agent;

        [SerializeField]
        private float _timeBeforePlay;

        [SerializeField]
        private Rigidbody[] _blowupTargets;

        [SerializeField]
        private Transform _explosionPos;

        [SerializeField]
        private TMP_Text _langText;

        private bool _isPendingPlay;
        private bool _isPendingCredits;

        private void Awake()
        {
            Translate.Instance.SetLanguages(new string[] { "english", "french" });

            _langText.text = "Français";
        }

        public void Play()
        {
            if (_isPendingPlay) return;

            _isPendingPlay = true;

            _agent.Go();

            StartCoroutine(PlayCoroutine());
        }

        public void Credits()
        {
            if (_isPendingCredits) return;

            _isPendingCredits = true;

            foreach (var r in _blowupTargets)
            {
                r.AddExplosionForce(1000f, _explosionPos.position, 1000f);
            }
        }

        public void UpdateLanguage()
        {
            if (Translate.Instance.CurrentLanguage == "english")
            {
                Translate.Instance.CurrentLanguage = "french";
                _langText.text = "English";
            }
            else if (Translate.Instance.CurrentLanguage == "french")
            {
                Translate.Instance.CurrentLanguage = "english";
                _langText.text = "Français";
            }
        }

        private IEnumerator PlayCoroutine()
        {
            yield return new WaitForSeconds(_timeBeforePlay);

            SceneManager.LoadScene("Main");
        }
    }
}