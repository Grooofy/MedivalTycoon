using System.Collections;
using UnityEngine;

public class Visitor : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private GameObject _sleepEffect;
    
    public int RequiredOrders { get; private set; }
    public float PreparationTime { get; private set; }
    public bool IsReadyToCarry { get; private set; }

    private Vector3 _targetPosition;
    private bool _isMoving;
    private Coroutine _movementCoroutine;

    public void Initialize(int orders, float prepTime)
    {
        RequiredOrders = orders;
        PreparationTime = prepTime;
        _sleepEffect.SetActive(false);
    }

    public void MoveToSeat(Vector3 target)
    {
        _targetPosition = target;
        StartCoroutine(MovementRoutine());
    }
    
    public void MoveToPosition(Vector3 target)
    {
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        _movementCoroutine = StartCoroutine(MoveCoroutine(target));
    }

    private IEnumerator MoveCoroutine(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                _moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    private IEnumerator MovementRoutine()
    {
        _isMoving = true;
        while(Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        _isMoving = false;
    }

    public void CompleteOrder()
    {
        _sleepEffect.SetActive(true);
        IsReadyToCarry = true;
    }
}
