using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PropseMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public UnityAction MovementOver; 

    public void MoveToPoint(Vector3 _pointPosition, GameObject barrel, bool isMoved)
    {
        StartCoroutine(MovedToPoint(_pointPosition, barrel, isMoved));
    }


    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel, bool isMoved)
    {
        while (IsMinDistance(barrel.transform.position, transform.position))
        {
            MovedTo(barrel, transform.position);
            yield return null;
        }
        barrel.transform.SetParent(transform);
        while (isMoved && IsMinDistance(barrel.transform.position, _pointPosition))
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

}
