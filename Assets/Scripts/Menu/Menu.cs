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

        private bool _isPendingPlay;

        public void Play()
        {
            if (_isPendingPlay) return;

            _isPendingPlay = true;

            _agent.Go();

            StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            yield return new WaitForSeconds(_timeBeforePlay);

            SceneManager.LoadScene("Main");
        }
    }
}