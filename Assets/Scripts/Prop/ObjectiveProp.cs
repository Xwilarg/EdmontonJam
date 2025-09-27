using EdmontonJam.Player;
using EdmontonJam.SO;
using Sketch.FPS;
using UnityEngine;
using UnityEngine.Assertions;

namespace EdmontonJam.Prop
{
    public class ObjectiveProp : MonoBehaviour, IInteractable
    {
        private ObjectivePropInfo _info;

        public void InitPropInfo(ObjectivePropInfo info)
        {
            Assert.IsNull(_info);

            _info = info;
            var go = Instantiate(info.Model, transform);
            go.transform.localPosition = Vector3.zero;
        }

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
            var cpc = (CustomPlayerController)pc;

            cpc.HoldObject(this);
        }

        public string InteractionVerb(PlayerController pc)
        {
            return "interaction_take";
        }
    }
}