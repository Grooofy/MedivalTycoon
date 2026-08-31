using Characters;
using Events;
using Tutorial;
using UnityEngine;

public class BarrelTaker : MonoBehaviour
{
    private LayerMask _handLayer;
    private IPropsMover _regulating;
    private Hand _currentHand;
    private Coroutine _activeCoroutine;

    private float _detectionRadius = 0.35f;
    private bool _hasGiven = false;

    public void Initialize(IPropsMover regulating, LayerMask handLayer)
    {
        _regulating = regulating;
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
                    var amount = Mathf.Min(_currentHand.Amount, _regulating.GetEmptyPointsCount());
                    if (amount == 0) return;

                    var props = _currentHand.GetTo(amount);

                    if (props == null || props.Count == 0) return;

                    _regulating.RegisterProps(props);                    
                    _activeCoroutine = StartCoroutine(_regulating.FillingPoints());                   
                    _hasGiven = true;
                    EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.MoveBarrel });
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