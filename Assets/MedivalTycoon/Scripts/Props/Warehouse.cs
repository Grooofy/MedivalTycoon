using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    [SerializeField] private Transform _issueArea;
    [SerializeField] private float _step;
    private float _currentStep = 0;
    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private Barrel[] _barrelsList;
    private WaitForSeconds _delay = new WaitForSeconds(0.25f);


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            IssueObjects(hand, hand.GetNumberWearableObjects());            
        }
    }

    private void Awake()
    {
        CreateQueue();
    }

    private void IssueObjects(Hand hand, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Barrel barrel = TryGetBarrel();
            //barrel.ChangePosition(hand.transform, hand.transform.position, _currentStep);
            CalculateStep();
        }
    }

    private IEnumerator RaiseObjects(Hand lift, WaitForSeconds delay)
    {
        while (_barrels.Count != 0)
        {
            lift.TryRaiseObject(TryGetBarrel());

            yield return delay;
        }
    }

    private void CalculateStep()
    {
        _currentStep += _step;
    }


    private void CreateQueue()
    {
        _barrelsList = GetComponentsInChildren<Barrel>();

        foreach (var barrel in _barrelsList)
        {
            _barrels.Enqueue(barrel);
        }
    }

    private Barrel TryGetBarrel()
    {
        if (_barrels.Count == 0)
        {
            return null;
        }
        return _barrels.Dequeue();
    }

}
