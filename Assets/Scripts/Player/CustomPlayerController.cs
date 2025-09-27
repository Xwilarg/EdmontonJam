using EdmontonJam.Manager;
using EdmontonJam.Prop;
using Sketch.FPS;
using Sketch.Translation;
using UnityEngine;
using UnityEngine.Assertions;

namespace EdmontonJam.Player
{
    public class CustomPlayerController : PlayerController
    {
        [SerializeField]
        private Transform _hands;

        public ObjectiveProp HoldedObject { set; get; }

        public void HoldObject(ObjectiveProp p)
        {
            Assert.IsNull(HoldedObject);

            HoldedObject = p;
            p.transform.parent = _hands.transform;
            p.transform.localPosition = Vector3.zero;

            RemoveInteraction(p);
            p.enabled = false;

            if (!GameManager.Instance.IsChasing) GameManager.Instance.IsChasing = true;
        }

        public override string GetInteractionText(string interactionVerb)
        {
            return Translate.Instance.Tr("interactionText", _pInput.currentControlScheme == "Keyboard&Mouse" ? "E" : Translate.Instance.Tr("southButton"), Translate.Instance.Tr(interactionVerb));
        }

        public override string GetDenyText(string denySentence)
        {
            return Translate.Instance.Tr(denySentence);
        }
    }
}