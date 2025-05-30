using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TavernGuest : MonoBehaviour
{
    public enum GuestState
    {
        InQueue,
        MovingToSeat,
        WaitingForOrder,
        Drinking,
        Satisfied,
        Leaving
    }

    private GuestState currentState;

    [SerializeField] private List<GameObject> modelPrefab;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxWaitTime = 10f;

    public bool isInteractable = false;
    public Action<bool, int> Waiting;
    private Animator _animator;
    private int beerAmount;
    private float waitTimer;
    private Vector3 targetPosition;
    private Seat currentSeat;
    

    private void Start()
    {
        currentState = GuestState.InQueue;
        beerAmount = Random.Range(1, 5);
        var guestGameObject = Instantiate(modelPrefab[Random.Range(0, modelPrefab.Count)], transform);
        _animator = guestGameObject.GetComponent<Animator>();
        StartCoroutine(GuestBehavior());
    }

    private IEnumerator GuestBehavior()
    {
        while (true)
        {
            Debug.Log(currentState+"State");
            switch (currentState)
            {
                case GuestState.InQueue:
                    yield return null;
                    break;
                case GuestState.MovingToSeat:
                    _animator.SetTrigger("Walk");
                    MoveToTarget();
                    yield return null;
                    break;
                case GuestState.WaitingForOrder:
                    WaitOrder();
                    _animator.SetTrigger("Idle");
                    yield return null;
                    break;
                case GuestState.Drinking:
                    yield return null;
                    break;
                case GuestState.Satisfied:
                    _animator.SetTrigger("Sleep");
                    isInteractable = true;
                    yield break;
                case GuestState.Leaving:
                    LeaveTavern();
                    yield break;
            }

            yield return null;
        }
    }

    public void AssignSeat(Seat seat)
    {
        if (seat.IsOccupied == false) return;

        currentState = GuestState.MovingToSeat;
        currentSeat = seat;
        targetPosition = seat.transform.position;
        StartCoroutine(MoveToPosition(targetPosition));
    }

    public void MoveToQueuePosition(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target position is null!");
            return;
        }

        StartCoroutine(MoveToPosition(target.position));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
            yield return null;
        }

        if (currentState == GuestState.MovingToSeat)
        {
            currentState = GuestState.WaitingForOrder;
            targetPosition = Vector3.zero;
            waitTimer = 0f;
        }
        else if (currentState == GuestState.Leaving)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = GuestState.WaitingForOrder;
            targetPosition = Vector3.zero;
            waitTimer = 0f;
        }
    }

    private void WaitOrder()
    {
        waitTimer += Time.deltaTime;
        Waiting?.Invoke(true, beerAmount);
        
        if (waitTimer >= maxWaitTime)
        {
            currentState = GuestState.Leaving;
            Waiting?.Invoke(false, 0);
            currentSeat.Vacate();
        }
    }

    public void Drinking(Seat seat)
    {
        currentState = GuestState.Drinking;
        _animator.SetTrigger("Drink");
        StartCoroutine(seat.DeliverBeer(1));
    }

    public void OrderCompleted()
    {
        currentState = GuestState.Satisfied;
        Waiting?.Invoke(false, 0);
    }

    public void InteractWithGuard(Transform exit)
    {
        if (isInteractable)
        {
            currentState = GuestState.Leaving;
            transform.parent = exit;
            currentSeat.Vacate();
        }
    }

    private void LeaveTavern()
    {
        Destroy(gameObject);
    }

    public int GetBeerAmount() => beerAmount;
}