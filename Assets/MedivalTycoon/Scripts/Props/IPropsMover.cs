using System;
using System.Collections;
using System.Collections.Generic;
using Propses;
using UnityEngine;
using UnityEngine.Events;

public interface IPropsMover
{
    public void Initialize(string sourceId, BarrelPool barrelPool);
    public void CreatePoints(int cout, float offset, Vector3 spaceSize = new Vector3());
    public void RegisterProps(Stack<IProps> props);
    public void RegisterProp(IProps barrel);
   
    public IEnumerator FillingPoints();
    public Stack<IProps> GetTo(int amount);
}