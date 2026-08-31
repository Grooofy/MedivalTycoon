using Characters;
using Events;
using Tutorial;
using UnityEngine;

public class SleepVisitorGiver : MonoBehaviour
{
    private LayerMask _securityLayer;
    private IPropsMover _regulating;
    private Hand _currentHand;
    private Coroutine _activeCoroutine;

    private float _detectionRadius = 0.30f;

    public void Initialize(IPropsMover regulating, LayerMask handLayer)
    {
        _regulating = regulating;
        _securityLayer = handLayer;
    }

    public void CheckHits()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _securityLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (_currentHand != null) break;

                if (hit.TryGetComponent(out Hand hand))
                {
                    _currentHand = hand;
                    _currentHand.RegisterProps(_regulating);
                    EventBus.Raise(new TutorialStepCompleted { Step = TutorialStep.TakeVisitor });
                    _activeCoroutine = StartCoroutine(_currentHand.FillingPoints());
                }
            }
        }
        else if (_currentHand != null)
        {
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