using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visitors;

public class VisitorSeatPoint : MonoBehaviour
{
    private LayerMask _handLayer = LayerMask.GetMask();
   
    private TavernVisitor _currentVisitor;
    private float _detectionRadius = 0.35f;
    private bool _hasGiven = false;
    public Vector3 GetPosition()
    {
        return transform.position + transform.localPosition;
    }

    public void CheckHits()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _handLayer);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (_hasGiven) break;

                if (hit.TryGetComponent(out TavernVisitor visitor))
                {
                    Debug.Log("EEEE");
                    _currentVisitor = visitor;
                    _currentVisitor.ChangeState(StateEvent.Idle); 
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
