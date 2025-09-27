using EdmontonJam.Prop;
using EdmontonJam.SO;
using Sketch.Translation;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EdmontonJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        private bool _isChasing;
        public bool IsChasing
        {
            set
            {
                if (value)
                {
                    GetComponent<PlayerInputManager>().DisableJoining();
                }
                _isChasing = value;
            }
            get => _isChasing;
        }

        [SerializeField]
        private ObjectivePropInfo[] _props;

        private void Awake()
        {
            Instance = this;

            Translate.Instance.SetLanguages(new string[] { "english", "french" });
        }

        private void Start()
        {
            var mixedProps = _props.OrderBy(x => Random.value).ToArray();
            var spots = GameObject.FindObjectsByType<ObjectiveProp>(FindObjectsSortMode.None);

            if (spots.Length > mixedProps.Length)
            {
                spots = spots.OrderBy(x => Random.value).Take(mixedProps.Length).ToArray();
            }
            else if (mixedProps.Length > spots.Length)
            {
                mixedProps = mixedProps.Take(spots.Length).ToArray();
            }

            for (int i = 0; i < spots.Length; i++)
            {
                spots[i].InitPropInfo(mixedProps[i]);
            }
        }
    }
}