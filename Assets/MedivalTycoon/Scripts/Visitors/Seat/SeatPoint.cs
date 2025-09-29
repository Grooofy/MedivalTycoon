using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visitors;

public class SeatPoint : MonoBehaviour
{
    public Action<TavernVisitor> VisitorSet;
    private LayerMask _visitorsLayer;
    private TavernVisitor _currentVisitor;
    private float _detectionRadius = 0.05f;
    private bool _hasGiven = false;
    private bool _isInitialize;


    public void Initialize(LayerMask visitorMask)
    {
        _visitorsLayer = visitorMask;
        _isInitialize = true;
    }

    public Vector3 GetPosition()
    {
        return transform.position + transform.localPosition;
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
                    _currentVisitor = visitor;
                    _currentVisitor.ChangeState(StateEvent.Waite);
                    VisitorSet?.Invoke(_currentVisitor);
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
