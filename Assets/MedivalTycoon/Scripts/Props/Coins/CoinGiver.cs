using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Money
{
    public class CoinGiver : MonoBehaviour
    {
        private LayerMask _waiterLayer;

        private float _detectionRadius = 0.35f;
        private IPropsMover _regulating;
        private Hand _currentHand;
        private Coroutine _activeCoroutine;
        private bool _isActive;

        public void Initialize(IPropsMover regulating, LayerMask waiterLayer)
        {
            _regulating = regulating;
            _waiterLayer = waiterLayer;
        }

        public void CheckHits()
        {
            if (_isActive == false) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _waiterLayer);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (_currentHand != null) break;

                    if (hit.TryGetComponent(out Hand hand))
                    {
                        if (hand.CanAccept(_regulating.Type))
                        {
                            _currentHand = hand;
                            _currentHand.RegisterProps(_regulating);
                            _activeCoroutine = StartCoroutine(_currentHand.FillingPoints());
                        }
                            
                    }
                }
            }
            else if (_currentHand != null)
            {
                if (_activeCoroutine != null)
                {
                    StopCoroutine(_activeCoroutine);
                    _activeCoroutine = null;
                }

                _currentHand = null;
            }
        }

        public void SetActiveGameObject(bool value)
        {
            _isActive = value;
            gameObject.SetActive(value);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif
    }
}
