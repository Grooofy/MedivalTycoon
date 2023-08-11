using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PropseMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public UnityAction MovementOver;
    private Transform _endPointPosition;
    private GameObject _barrel;
    

    public void MoveThroughTwoPoints(Vector3 _pointPosition, GameObject barrel)
    {
        StartCoroutine(MovedToPoint(_pointPosition, barrel, true));
    }

    public void MoveThroughOnePoints(Transform _pointPosition, GameObject barrel)
    {
        _endPointPosition = _pointPosition;
        _barrel = barrel;
    }

    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel, bool isMoreOnePoint)
    {
        while (!IsMinDistance(barrel.transform.position, transform.position))
        {
            MovedTo(transform.position, barrel);
            yield return null;
        }
        barrel.transform.SetParent(transform);

        while (isMoreOnePoint && !IsMinDistance(barrel.transform.position, _pointPosition))
        {
            MovedTo(_pointPosition, barrel);
            yield return null;
        }
        MovementOver?.Invoke();
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }


    private void MovedTo(Vector3 positionEndPoint, GameObject item)
    {
        item.transform.position =
               Vector3.MoveTowards(item.transform.position, positionEndPoint, _speed * Time.deltaTime);
        
    }

    private void Update()
    {
        if (_endPointPosition != null)
        {
            MovedTo(_endPointPosition.position, _barrel);
            
            if (IsMinDistance(_barrel.transform.position, _endPointPosition.position))
            {
                _endPointPosition = null;
                _barrel.transform.SetParent(transform);
                MovementOver?.Invoke();
            }
        }
    }
}
