using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visitor : MonoBehaviour
{
    public enum State { Waiting, MovingToSeat, InProcess }
    
    [Header("Настройки")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float positionThreshold = 0.1f;

    private State _currentState;
    private int _requiredOrders;
    private Vector3 _targetPosition;
    private Seat _targetSeat;
    public event System.Action<Visitor> OnDestroyed;

    public State CurrentState => _currentState;
    public int RequiredOrders => _requiredOrders;

    public void Initialize(int orders)
    {
        _requiredOrders = orders;
        _currentState = State.Waiting;
    }
   
    public void MoveToPosition(Vector3 position, System.Action onComplete = null)
    {
        _currentState = State.MovingToSeat;
        StartCoroutine(MovementRoutine(position, onComplete));
    }

    private IEnumerator MovementRoutine(Vector3 target, System.Action callback)
    {
        while (Vector3.Distance(transform.position, target) > positionThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            RotateTowards(target);
            yield return null;
        }
        
        _currentState = State.Waiting;
        callback?.Invoke();
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void AssignSeat(Seat seat)
    {
        _targetSeat = seat;
        _currentState = State.InProcess;
        StartCoroutine(ProcessOrder());
    }

    private IEnumerator ProcessOrder()
    {
        yield return new WaitForSeconds(_requiredOrders * 2f);
        _targetSeat.Release();
        SeatManager.Instance.ReturnSeat(_targetSeat);
        OnDestroyed?.Invoke(this); // Уведомляем о уничтожении
        Destroy(gameObject);
    }
}