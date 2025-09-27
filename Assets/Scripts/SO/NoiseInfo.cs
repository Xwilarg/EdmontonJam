using UnityEngine;

namespace EdmontonJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/NoiseInfo", fileName = "NoiseInfo")]
    public class NoiseInfo : ScriptableObject
    {
        public float NoiseForce;
        public Material Material;
    }
}