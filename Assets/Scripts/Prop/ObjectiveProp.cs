using EdmontonJam.Manager;
using EdmontonJam.Player;
using EdmontonJam.SO;
using Sketch.FPS;
using UnityEngine;
using UnityEngine.Assertions;

namespace EdmontonJam.Prop
{
    public class ObjectiveProp : MonoBehaviour, IPickable
    {
        private ObjectivePropInfo _info;
        private bool _wasTaken;

        public void InitPropInfo(ObjectivePropInfo info)
        {
            Assert.IsNull(_info);

            _info = info;
            var go = Instantiate(info.Model, transform);
            go.transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 40f);
        }

        public void Pick(CustomPlayerController cpc)
        {
            var cpc = (CustomPlayerController)pc;

            _wasTaken = true;
            cpc.HoldObject(this);

            NoiseManager.Instance.SpawnNoise(transform.position, _info.AttachedNoise, null);
        }
    }
}