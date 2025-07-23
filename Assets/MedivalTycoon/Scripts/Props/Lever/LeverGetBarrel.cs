using Characters;
using UnityEngine;

namespace Lever
{
    public class LeverGetBarrel : MonoBehaviour
    {
        private GroundUI _uiObject;
        private BarrelBuffer _barrelBuffer;
        private SphereCollider _collider;

        public void Initialize(IPropsMover propsMover, SphereCollider collider, GroundUI uiObject)
        {
            _barrelBuffer = propsMover as BarrelBuffer;
            _collider = collider;
            _uiObject = uiObject;
            EventBus.Subscribe<PropsMoverFullingPointEvent>(TurnObject);
        }


        private void OnDestroy()
        {
            EventBus.Unsubscribe<PropsMoverFullingPointEvent>(TurnObject);
        }


        private void TurnObject(PropsMoverFullingPointEvent value)
        {
            _collider.enabled = !value.IsFull;
            
            if (value.IsFull)
                _uiObject.FadeOut();
            else
                _uiObject.FadeIn();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bartender bartender))
            {
                StartCoroutine(_barrelBuffer.FillingPoints());
            }
        }
    }
}