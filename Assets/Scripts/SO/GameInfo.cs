using UnityEngine;

namespace EdmontonJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInfo", fileName = "GameInfo")]
    public class GameInfo : ScriptableObject
    {
        public float LockpickReloadTime;
        public float MinDoorMagnitudeForNoise;
    }
}