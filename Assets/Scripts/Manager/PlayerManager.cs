using EdmontonJam.Player;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EdmontonJam.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private SpawnPoint[] _spawnPoints;

        private int _spawnId;

        private void Awake()
        {
            _spawnPoints = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        }

        public void OnPlayerJoin(PlayerInput p)
        {
            var cc = p.transform.parent.GetComponent<CharacterController>();
            cc.enabled = false;
            var targetSpawn = _spawnPoints[_spawnId++ % _spawnPoints.Length];
            p.transform.parent.position = targetSpawn.transform.position;
            cc.enabled = true;

            cc.GetComponent<CustomPlayerController>().AttachedSpawn = targetSpawn;
        }
    }
}
