using Events;
using System;
using UnityEngine;
using Visitors;

public class SeatPoint : MonoBehaviour
{   
    private LayerMask _visitorsLayer;
    private TavernVisitor _currentVisitor;
    private Seat _seat;
    private float _detectionRadius = 0.05f;
    private bool _hasGiven = false;
    private bool _isInitialize;


    public void Initialize(LayerMask visitorMask, Seat seat)
    {
        _visitorsLayer = visitorMask;
        _seat = seat;
        _isInitialize = true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void CheckHits()
    {
        if (_isInitialize == false) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _visitorsLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (_hasGiven) break; 

                if (hit.TryGetComponent(out TavernVisitor visitor))
                {
                    if (visitor.IsTrigger == false) break;

                    _currentVisitor = visitor;
                    _currentVisitor.ChangeState(StateEvent.Wait);
                    _seat.VisitorSet(_currentVisitor);

                    _hasGiven = true; 
                }
            }
        }
        else if (_hasGiven)
        {
            _hasGiven = false;
            _currentVisitor = null;
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
