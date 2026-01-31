using System;
using Characters;
using JetBrains.Annotations;
using UnityEngine;

namespace Lever
{
    public class LeverGetBarrel : MonoBehaviour
    {
        private LayerMask _handLayer;
        private IPropsMover _regulating;
        private Coroutine _activeCoroutine;
        private GroundUI _uiObject;

        private float _detectionRadius = 0.30f;
        private bool _hasGiven = false;

        public void Initialize(IPropsMover regulating, GroundUI uiObject, LayerMask layer)
        {
            _handLayer = layer;
            _regulating = regulating;
            _uiObject = uiObject;
            EventBus.Subscribe<PropsMoverFullingPointEvent>(TurnObject);
        }

        public void CheckHits()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _handLayer);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (_hasGiven) break;

                    if (hit.TryGetComponent(out Bartender bartender))
                    {
                        if (_activeCoroutine != null)
                        {
                            StopCoroutine(_activeCoroutine);
                        }

                        _activeCoroutine = StartCoroutine(_regulating.FillingPoints());
                        _hasGiven = true;
                    }
                }
            }

            else if (_hasGiven)
            {
                _hasGiven = false;

                if (_activeCoroutine != null)
                {
                    StopCoroutine(_activeCoroutine);
                    _activeCoroutine = null;
                }
            }
        }

        private void TurnObject(PropsMoverFullingPointEvent value)
        {
            if (value.IsFull)
                _uiObject.FadeOut();
            else
                _uiObject.FadeIn();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PropsMoverFullingPointEvent>(TurnObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif



    }
}


/*    Старый Вариант Удалить после проверки!!!
    
     
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
               _barrelBuffer.IsTake = true;
               StartCoroutine(_barrelBuffer.FillingPoints());
           }
       }

       private void OnTriggerExit(Collider other)
       {
           if (other.TryGetComponent(out Bartender bartender))
               _barrelBuffer.IsTake = false;
       }*/