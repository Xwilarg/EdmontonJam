using Sketch.Translation;
using UnityEngine;

namespace EdmontonJam.Manager
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Translate.Instance.SetLanguages(new string[] { "english", "french" });
        }
    }
}