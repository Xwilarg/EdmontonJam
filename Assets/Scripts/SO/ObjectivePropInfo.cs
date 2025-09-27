using UnityEngine;

namespace EdmontonJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/ObjectivePropInfo", fileName = "ObjectivePropInfo")]
    public class ObjectivePropInfo : ScriptableObject
    {
        public GameObject Model;

        public NoiseInfo AttachedNoise;
    }
}