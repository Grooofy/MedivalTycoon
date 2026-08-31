using Characters;
using Events;
using JetBrains.Annotations;
using System;
using Tutorial;
using UnityEngine;

namespace Lever
{
    public class LeverGetBarrel : MonoBehaviour
    {
        private static float DetectionRadius = 0.30f;
        private static Vector3 Offset = new Vector3(0.2f, 0, -0.4f);
        private LayerMask _handLayer;
        private IPropsMover _regulating;
        private Coroutine _activeCoroutine;
        private GroundUI _uiObject;

        private Vector3 _position;
        private bool _hasGiven = false;

        public void Initialize(IPropsMover regulating, GroundUI uiObject, LayerMask layer)
        {
            _handLayer = layer;
            _regulating = regulating;
            _uiObject = uiObject;
            _position = transform.position + Offset;
            EventBus.Subscribe<PropsMoverFullingPointEvent>(TurnObject);
        }

        public void CheckHits()
        {
            Collider[] hits = Physics.OverlapSphere(_position, DetectionRadius, _handLayer);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (_hasGiven) break;

                    if (hit.TryGetComponent(out Bartender bartender))
                    {
                        EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.CreateBarrel });
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
            Gizmos.DrawWireSphere(_position, DetectionRadius);
        }
#endif
    }
}
