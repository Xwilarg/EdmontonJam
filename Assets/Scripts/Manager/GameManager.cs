using EdmontonJam.Prop;
using EdmontonJam.SO;
using Sketch.Translation;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
                    ResourceManager.Instance.SetWarningText();
                    foreach (var jp in _joinPrompts) jp.SetActive(false);
                    _light.SetActive(false);
                }
                _isChasing = value;
            }
            get => _isChasing;
        }

        private int _itemLeft = 3;
        public int ItemLeft
        {
            set
            {
                if (value == 0)
                {
                    ResourceManager.Instance.ShowVictoryWarning();
                }
                _itemLeft = value;
                _itemLeftText.text = $"{3 - value}/3";
            }
            get => _itemLeft;
        }

        [SerializeField]
        private ObjectivePropInfo[] _props;

        [SerializeField]
        private GameObject[] _joinPrompts;

        [SerializeField]
        private GameObject _light;

        [SerializeField]
        private TMP_Text _itemLeftText;

        private float _timer;

        private void Awake()
        {
            Instance = this;

            if (!SceneManager.GetAllScenes().Any(x => x.name == "PlayerDebugLevel"))
            {
                if (!SceneManager.GetAllScenes().Any(x => x.name == "Level")) SceneManager.LoadScene("Level", LoadSceneMode.Additive);
                if (!SceneManager.GetAllScenes().Any(x => x.name == "House")) SceneManager.LoadScene("House", LoadSceneMode.Additive);
            }
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
            //ItemLeft = spots.Length;
        }

        private void Update()
        {
            if (IsChasing && _timer < 1f)
            {
                _timer += Time.deltaTime * .1f;
                foreach (var m in ResourceManager.Instance.GameInfo._horrorMats)
                {
                    m.SetFloat("_HorrorLevel", Mathf.Clamp01(_timer));
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var m in ResourceManager.Instance.GameInfo._horrorMats)
            {
                m.SetFloat("_HorrorLevel", 0f);
            }
        }
    }
}