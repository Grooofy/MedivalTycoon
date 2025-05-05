using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TavernGuest : MonoBehaviour
{
    public enum GuestState { InQueue, MovingToSeat, WaitingForOrder, Satisfied, Leaving }
    private GuestState currentState;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxWaitTime = 10f;
    private int beerAmount;
    private float waitTimer;
    private Transform targetPosition;
    private Seat currentSeat;
    private bool isInteractable = false;

    private void Start()
    {
        currentState = GuestState.InQueue;
        beerAmount = Random.Range(1, 5);
        StartCoroutine(GuestBehavior());
    }

    private void Update()
    {
        Debug.Log(currentState);
        Debug.Log(beerAmount);
    }

    private IEnumerator GuestBehavior()
    {
        while (true)
        {
            switch (currentState)
            {
                case GuestState.InQueue:
                    yield return null;
                    break;
                case GuestState.MovingToSeat:
                    MoveToTarget();
                    yield return null;
                    break;
                case GuestState.WaitingForOrder:
                    WaitOrder();
                    yield return null;
                    break;
                case GuestState.Satisfied:
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
        currentState = GuestState.MovingToSeat;
        currentSeat = seat;
        targetPosition = seat.transform;
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
            yield return null;
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            currentState = GuestState.WaitingForOrder;
            targetPosition = null;
            waitTimer = 0f;
        }
    }

    private void WaitOrder()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= maxWaitTime)
        {
            currentState = GuestState.Leaving;
            currentSeat.Vacate();
        }
    }

    public void OrderCompleted()
    {
        currentState = GuestState.Satisfied;
    }

    public void InteractWithGuard(Transform exit)
    {
        if (isInteractable)
        {
            currentState = GuestState.Leaving;
            targetPosition = exit;
        }
    }

    private void LeaveTavern()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            Destroy(gameObject);
    }

    public int GetBeerAmount() => beerAmount;
}