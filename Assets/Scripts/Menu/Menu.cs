using EdmontonJam.Grandma;
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

        private bool _isPendingPlay;
        private bool _isPendingCredits;

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

        private IEnumerator PlayCoroutine()
        {
            yield return new WaitForSeconds(_timeBeforePlay);

            SceneManager.LoadScene("Main");
        }
    }
}