using Characters;
using Events;
using Tutorial;
using UnityEngine;

public class BarrelGiver : MonoBehaviour
{
    private LayerMask _bartenderLayer;

    private float _detectionRadius = 0.35f;
    private IPropsMover _regulating;
    private Hand _currentHand;
    private Coroutine _activeCoroutine;
    private bool _isActive;

    public void Initialize(IPropsMover regulating, LayerMask handLayer)
    {
        _regulating = regulating;
        _bartenderLayer = handLayer;
        _isActive = true;
    }

    public void CheckHits()
    {
        if (_isActive == false) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _bartenderLayer);

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
                        EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.TakeBarrel });
                    }
                }
            }
        }
        else if (_currentHand != null)
        {
            _activeCoroutine = null;
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