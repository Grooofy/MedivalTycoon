using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PropseMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public UnityAction MovementOver;
    private Transform PointPosition;
    private GameObject Barrel;
    

    public void MoveThroughTwoPoints(Vector3 _pointPosition, GameObject barrel)
    {
        StartCoroutine(MovedToPoint(_pointPosition, barrel, true));
    }

    public void MoveThroughOnePoints(Transform _pointPosition, GameObject barrel)
    {
        PointPosition = _pointPosition;
        Barrel = barrel;
        //StartCoroutine(MovedToPoint(_pointPosition, barrel));

    }


    private IEnumerator MovedToPoint(Transform _pointPosition, GameObject barrel)
    {
        barrel.transform.SetParent(transform);
        
        while (IsMinDistance(barrel.transform.position, _pointPosition.localPosition))
        {
            MovedTo(barrel, _pointPosition.localPosition);
            Debug.Log(barrel.transform.position + "Бочка");
            yield return null;
        }
        MovementOver?.Invoke();
    }

    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel, bool isMoreOnePoint)
    {
        while (IsMinDistance(barrel.transform.position, transform.position))
        {
            MovedTo(barrel, transform.position);
            yield return null;
        }
        barrel.transform.SetParent(transform);

        while (isMoreOnePoint && IsMinDistance(barrel.transform.position, _pointPosition))
        {
            MovedTo(barrel, _pointPosition);
            yield return null;
        }
        MovementOver?.Invoke();
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) > minDistance;
    }


    private void MovedTo(GameObject item, Vector3 positionEndPoint)
    {
        item.transform.position =
               Vector3.MoveTowards(item.transform.position, positionEndPoint, _speed * Time.deltaTime);
        
    }

    private void Update()
    {
        if (PointPosition != null)
        {
            MovedTo(Barrel, PointPosition.position);
        }
    }
}
