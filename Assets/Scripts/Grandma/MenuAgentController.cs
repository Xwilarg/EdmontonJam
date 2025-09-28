using UnityEngine;
using UnityEngine.AI;

namespace EdmontonJam.Grandma
{
    public class MenuAgentController : MonoBehaviour
    {
        [SerializeField]
        private Transform _destination;

        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Go()
        {
            _navMeshAgent.SetDestination(_destination.position);
        }
    }
}