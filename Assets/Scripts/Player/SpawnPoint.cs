using UnityEngine;

namespace EdmontonJam.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
        }
    }
}