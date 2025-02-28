using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelToBeer : MonoBehaviour, IPropsMover
{
    [SerializeField] private Point _barrelPoint;
    private Queue<Props> _props = new Queue<Props>();
    
    

    public void RegisterProps(Queue<Props> props)
    {
        _props = props;
    }

    public IEnumerator FillingPoints()
    {
        throw new NotImplementedException();
    }

    public Queue<Props> GetTo(int amount)
    {
        throw new NotImplementedException();
    }
}
