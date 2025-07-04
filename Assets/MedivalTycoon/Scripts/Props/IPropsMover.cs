using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPropsMover
{
    public void CreatePoints(int cout, float offset);
    public void RegisterProps(Queue<Props> props);
    public void RegisterProp(Props props);
   
    public IEnumerator FillingPoints();
    public Queue<Props> GetTo(int amount);
}