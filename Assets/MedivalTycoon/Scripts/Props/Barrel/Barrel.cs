using UnityEngine;
using DG.Tweening;

public class Barrel : MonoBehaviour, IProps
{    
    [SerializeField] private Animator _animator;
    [SerializeField] private float _jumpForse;

    [Range(0, 10)]
    [SerializeField] private int _numJump;

    [Range(0.5f, 5)]
    [SerializeField] private float _duration;

    private const string _isStop = "IsStop";

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void Move(Vector3 startPoint, Vector3 endPoint)
    {
        transform.position = Vector3.MoveTowards(startPoint, endPoint, _jumpForse * Time.deltaTime);

        if (IsMinDistance(gameObject.transform.position, endPoint))
        {
            StopRotationAnimation();
        }
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

