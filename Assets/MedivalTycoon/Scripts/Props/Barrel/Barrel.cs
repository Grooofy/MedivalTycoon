using UnityEngine;
using DG.Tweening;

public class Barrel : MonoBehaviour, IProps
{    
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;

    private const string _isStop = "IsStop";
    private Vector3 _startPoint;
   
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void Move(Vector3 endPoint)
    {
        _startPoint = GetPosition();
        transform.position = Vector3.MoveTowards(_startPoint, endPoint, _speed * Time.deltaTime);

        if (IsMinDistance(gameObject.transform.position, endPoint))
        {
            StopRotationAnimation();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetNewParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    private void StopRotationAnimation()
    {
        _animator.SetBool(_isStop, true);
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }

}

