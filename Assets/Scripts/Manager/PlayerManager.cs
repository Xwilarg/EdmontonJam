using EdmontonJam.Player;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EdmontonJam.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private Transform[] _spawnPoints;

        private int _spawnId;

        private void Awake()
        {
            _spawnPoints = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).Select(x => x.transform).ToArray();
        }

        public void OnPlayerJoin(PlayerInput p)
        {
            var cc = p.transform.parent.GetComponent<CharacterController>();
            cc.enabled = false;
            p.transform.parent.position = _spawnPoints[_spawnId++ % _spawnPoints.Length].position;
            cc.enabled = true;
        }
    }
}
