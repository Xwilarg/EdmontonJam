using Sketch.Translation;
using UnityEngine;

namespace EdmontonJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public static bool IsChasing { private set; get; }

        private void Awake()
        {
            Instance = this;

            Translate.Instance.SetLanguages(new string[] { "english", "french" });
        }
    }
}