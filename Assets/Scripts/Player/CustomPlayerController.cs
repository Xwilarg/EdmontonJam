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
            return Translate.Instance.Tr("interactionText", _pInput.currentControlScheme == "Keyboard&Mouse" ? "E" : Translate.Instance.Tr("southButton"), Translate.Instance.Tr(interactionVerb));
        }

        public override string GetDenyText(string denySentence)
        {
            return Translate.Instance.Tr(denySentence);
        }
    }
}