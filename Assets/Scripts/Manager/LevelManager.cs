using UnityEngine;

namespace EdmontonJam.Manager
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { private set; get; }

        [SerializeField]
        private Transform _map, _minimap;

        private void Awake()
        {
            Instance = this;
        }

        public void MoveToMinimapPosition(Vector3 position, GameObject go)
        {
            var p = (position - _map.position) + _minimap.position;
            p.y = 1f;
            go.transform.position = p;
        }
    }
}