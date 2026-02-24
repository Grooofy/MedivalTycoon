using Characters;
using UnityEngine;

namespace Beers
{
    public class BeerTaker : MonoBehaviour
    {
        private LayerMask _handLayer;
        private IPropsMover _regulating;
        private Hand _currentHand;
        private Coroutine _activeCoroutine;

        private float _detectionRadius = 0.35f;
        private bool _hasGiven = false;
        private bool _isActive;

        public void Initialize(IPropsMover regulating, LayerMask handLayer)
        {
            _regulating = regulating;
            _handLayer = handLayer;
        }

        public void CheckHits()
        {
            if (_isActive == false) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _handLayer);
           
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (_hasGiven) break;

                    if (hit.TryGetComponent(out Hand hand))
                    {
                        if (hand.CanAccept(_regulating.Type))
                        {
                            _currentHand = hand;
                            var amount = Mathf.Min(_currentHand.Amount, _regulating.GetEmptyPointsCount());
                            var props = _currentHand.GetTo(amount);
                            if (props == null || props.Count == 0) return;
                            _regulating.RegisterProps(props);
                             StartCoroutine(_regulating.FillingPoints());
                            _hasGiven = true;
                        }
                            
                    }                   
                }
            }
            else if (_hasGiven)
            {
                _hasGiven = false;

                if (_activeCoroutine != null)
                    _activeCoroutine = null;

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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif
    }
}