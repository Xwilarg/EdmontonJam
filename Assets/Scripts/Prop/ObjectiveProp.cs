using EdmontonJam.Player;
using Sketch.FPS;
using UnityEngine;

namespace EdmontonJam.Prop
{
    public class ObjectiveProp : MonoBehaviour, IInteractable
    {
        public GameObject GameObject => gameObject;

        public bool CanInteract(PlayerController pc)
        {
            var cpc = (CustomPlayerController)pc;

            return cpc.HoldedObject == null;
        }

        public string DenySentence(PlayerController pc)
        {
            return "alreadyHold";
        }

        public void Interact(PlayerController pc)
        {
            
        }

        public string InteractionVerb(PlayerController pc)
        {
            return "interaction_take";
        }
    }
}