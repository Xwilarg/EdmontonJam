using EdmontonJam.Grandma;
using EdmontonJam.SO;
using Sketch.Translation;
using System.Collections;
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
        private GameInfo _info;

        private bool _isPendingPlay;
        private bool _isPendingCredits;

        private void Awake()
        {
            foreach (var m in _info._horrorMats)
            {
                m.SetFloat("_HorrorLevel", 0f);
            }

            Translate.Instance.SetLanguages(new string[] { "english", "french", "spanish"/*, "arabic"*/ });
            /*Translate.Instance.TranslationHook = (s) =>
            {
                if (Translate.Instance.CurrentLanguage == "arabic") return ArabicFixer.Fix(s);
                return s;
            };*/
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

        public void SetEnglish() => Translate.Instance.CurrentLanguage = "english";
        public void SetFrench() => Translate.Instance.CurrentLanguage = "french";
        public void SetSpanish() => Translate.Instance.CurrentLanguage = "spanish";
        public void SetArabic() => Translate.Instance.CurrentLanguage = "arabic";

        private IEnumerator PlayCoroutine()
        {
            yield return new WaitForSeconds(_timeBeforePlay);

            SceneManager.LoadScene("Main");
        }
    }
}