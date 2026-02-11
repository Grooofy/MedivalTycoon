using Characters;
using UnityEngine;

public class SleepVisitorsTaker : MonoBehaviour
{
    private LayerMask _handLayer;
    private SleepVisitorBuffer _sleepVisitorBuffer;
    private Hand _currentHand;
    private Coroutine _activeCoroutine;

    private float _detectionRadius = 0.35f;
    private bool _hasGiven = false;

    public void Initialize(SleepVisitorBuffer exitPoint, LayerMask handLayer)
    {
        _sleepVisitorBuffer = exitPoint;
        _handLayer = handLayer;
    }

    public void CheckHits()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _handLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (_hasGiven) break;

                if (hit.TryGetComponent(out Hand hand))
                {
                    _currentHand = hand;
                    var props = _currentHand.GetTo(_currentHand.Amount);
                    if (props == null || props.Count == 0) return;
                    _sleepVisitorBuffer.RegisterProps(props);
                    _activeCoroutine = StartCoroutine(_sleepVisitorBuffer.FillingPoints());
                    _hasGiven = true;
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
#endif
}