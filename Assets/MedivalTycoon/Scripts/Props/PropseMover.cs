using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PropseMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public UnityAction MovementOver;
    public UnityAction MovedEnded;
    private Transform _endPointPosition;
    private GameObject _barrel;
    

    private void Update()
    {
        if (_endPointPosition != null)
        {
            MovedTo(_endPointPosition.position, _barrel);
            
            if (IsMinDistance(_barrel.transform.position, _endPointPosition.position))
            {
                _barrel.transform.SetParent(transform);
                MovedEnded?.Invoke();
                _endPointPosition = null;
            }
        }
    }
    public void MoveThroughOnePoints(Transform pointPosition, GameObject barrel)
    {
        _endPointPosition = pointPosition;
        _barrel = barrel;
    }
    
    public void MoveThroughTwoPoints(Vector3 _pointPosition, GameObject barrel)
    {
        StartCoroutine(MovedToPoint(_pointPosition, barrel, true));
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

}
