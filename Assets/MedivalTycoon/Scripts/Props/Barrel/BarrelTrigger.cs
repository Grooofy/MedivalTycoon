using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private List<Barrel> _barrels;
    private Queue<Barrel> _queueBarrels = new Queue<Barrel>();

    private void Start()
    {
        for (int i = 0; i < _barrels.Count; i++)
        {
            _queueBarrels.Enqueue(_barrels[i]);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            hand.IsTaked = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            hand.IsTaked = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            if (_queueBarrels == null)
            {
                return;
            }
            hand.Take(_queueBarrels.Dequeue());
        }
    }
}
