using System.Linq;
using UnityEngine;

namespace EdmontonJam.Minimap
{
    public class SetAllToMat : MonoBehaviour
    {
        [SerializeField]
        private Material _matWall, _matFloor;

        private void Awake()
        {
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.name.Contains("wall", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    r.materials = Enumerable.Repeat(_matWall, r.materials.Length).ToArray();
                }
                else
                {
                    r.materials = Enumerable.Repeat(_matFloor, r.materials.Length).ToArray();
                }
            }
        }
    }
}