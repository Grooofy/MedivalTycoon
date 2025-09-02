using Beers;
using Characters;
using Events;
using UnityEngine;

namespace Lever
{
    public class LeverBarrelToBeer : MonoBehaviour
    {
        private GroundUI _uiObject;
        private BarrelBeerBuffer _barrelBeerBuffer;
        private SphereCollider _collider;


        public void Initialize(IPropsMover propsMover, SphereCollider collider, GroundUI uiObject)
        {
            _barrelBeerBuffer = propsMover as BarrelBeerBuffer;
            _collider = collider;
            _uiObject = uiObject;
        }

        private void TurnObject(CharacterGetBeer value)
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
              //  _barrelBeerBuffer.IsTake = true;
                StartCoroutine(_barrelBeerBuffer.FillingPoints());
            }
        }

        private void OnTriggerExit(Collider other)
        {
           // if (other.TryGetComponent(out Bartender bartender))
              //  _barrelBeerBuffer.IsTake = false;
        }
    }
}