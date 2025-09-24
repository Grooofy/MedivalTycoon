using Characters;
using UnityEngine;

public class BarrelGiver : MonoBehaviour
{
    private LayerMask _handLayer;

    private float _detectionRadius = 0.35f;
    private IPropsMover _regulating;
    private Hand _currentHand;
    private Coroutine _activeCoroutine;
    private bool _isActive;

    public void Initialize(IPropsMover regulating, LayerMask handLayer)
    {
        _regulating = regulating;
        _handLayer = handLayer;
        _isActive = true;
    }

    public void CheckHits()
    {
        if (_isActive == false) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _handLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (_currentHand != null) break;

                if (hit.TryGetComponent(out Hand hand))
                {
                    _currentHand = hand;
                    _currentHand.RegisterProps(_regulating);
                    _activeCoroutine = StartCoroutine(_currentHand.FillingPoints());
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
#endif
}