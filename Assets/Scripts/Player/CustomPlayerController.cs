using EdmontonJam.Prop;
using Sketch.FPS;
using Sketch.Translation;
using UnityEngine;

namespace EdmontonJam.Player
{
    public class CustomPlayerController : PlayerController
    {
        public ObjectiveProp HoldedObject { set; get; }

        public override string GetInteractionText(string interactionVerb)
        {
            Debug.Log(_pInput.currentControlScheme);
            return Translate.Instance.Tr("interactionText", "", Translate.Instance.Tr(interactionVerb));
        }

        public override string GetDenyText(string denySentence)
        {
            return Translate.Instance.Tr(denySentence);
        }
    }
}