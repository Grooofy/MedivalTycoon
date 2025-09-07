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
        private BeerBuffer _beerBuffer;
        private SphereCollider _collider;
        private Coroutine _delayBarrelReset;


        public void Initialize(IPropsMover beerBuffer, IPropsMover propsMover, SphereCollider collider, GroundUI uiObject)
        {
            _beerBuffer = beerBuffer as BeerBuffer;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Waiter waiter))
            {
                if (_beerBuffer.GetEmptyPointsCount() == 0) return;
                _barrelBeerBuffer.IsTake = true;
                StartCoroutine(_barrelBeerBuffer.ResetBarrel());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Waiter waiter))
                if (_beerBuffer.GetEmptyPointsCount() == 0) 
                    _barrelBeerBuffer.IsTake = false;
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