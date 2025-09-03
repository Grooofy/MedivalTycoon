using System;
using Beers;
using Characters;
using Events;
using UnityEngine;

namespace Lever
{
    public class LeverBeer : MonoBehaviour
    {
        private GroundUI _uiObject;
        private BarrelBeerBuffer _barrelBeerBuffer;
        private SphereCollider _collider;
        private Coroutine _delayBarrelReset;


        public void Initialize(IPropsMover propsMover, SphereCollider collider, GroundUI uiObject)
        {
            _barrelBeerBuffer = propsMover as BarrelBeerBuffer;
            _collider = collider;
            _uiObject = uiObject;
            EventBus.Subscribe<BeerBufferOpen>(TurnObject);
        }

        private void TurnObject(BeerBufferOpen value)
        {
            _collider.enabled = value.IsEmpty;
            
            if (value.IsEmpty)
                _uiObject.FadeIn();
            else
                _uiObject.FadeOut();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Waiter waiter))
            {
                _barrelBeerBuffer.IsTake = true;
              StartCoroutine(_barrelBeerBuffer.ResetBarrel());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Waiter waiter))
                _barrelBeerBuffer.IsTake = false;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BeerBufferOpen>(TurnObject);
        }       
    }
}