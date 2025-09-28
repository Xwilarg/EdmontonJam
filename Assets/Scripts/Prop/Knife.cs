using EdmontonJam.Player;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class Knife : MonoBehaviour, IPickable
    {
        public void Pick(CustomPlayerController cpc)
        {
            cpc.HasKnife = true;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, 40f* Time.deltaTime);
        }

    }

}


