using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerCreator : MonoBehaviour, IPropsMover
{
    private Queue<Props> _props = new Queue<Props>();
    
    public void RegisterProps(Queue<Props> props)
    {
        if (props == null)
        {
            Debug.LogError("Null register props!");
            return;
        }

        if (props.Count == 0)
        {
            Debug.LogWarning("Empty props queue passed to RegisterProps.");
            return;
        }

        foreach (var prop in props)
        {
            if (prop == null)
            {
                Debug.LogWarning("Null prop found in the queue. Skipping.");
                continue; 
            }

            _props.Enqueue(prop);
        }

        Debug.Log($"Successfully registered {props.Count} props in Regulating.");
    }

    public IEnumerator FillingPoints()
    {
        throw new System.NotImplementedException();
    }

    public Queue<Props> GetTo(int amount)
    {
        throw new System.NotImplementedException();
    }
}
