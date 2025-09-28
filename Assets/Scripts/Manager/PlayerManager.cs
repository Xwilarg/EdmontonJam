using EdmontonJam.Grandma;
using EdmontonJam.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EdmontonJam.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _mainCamera;

        private SpawnPoint[] _spawnPoints;

        private int _spawnId;

        private void Start()
        {
            _spawnPoints = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

            Debug.Log($"Spawn points found: {_spawnPoints.Length}");
        }

        public void OnPlayerJoin(PlayerInput p)
        {
            var cc = p.transform.parent.GetComponent<CharacterController>();
            cc.enabled = false;
            var targetSpawn = _spawnPoints[_spawnId++ % _spawnPoints.Length];
            p.transform.parent.position = targetSpawn.transform.position;
            p.transform.rotation = targetSpawn.transform.rotation;
            cc.enabled = true;

            cc.GetComponent<CustomPlayerController>().AttachedSpawn = targetSpawn;

            GrandmaController.instance.Register(cc.GetComponent<CustomPlayerController>());

            _mainCamera.SetActive(false);
        }
    }
}
