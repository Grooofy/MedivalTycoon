using System;
using System.Collections;
using System.Collections.Generic;
using MedivalTycoon;
using Propses;
using UnityEngine;
using UnityEngine.Events;

public interface IPropsMover
{    
    public void CreatePoints(int cout, float offset, Vector3 spaceSize = new Vector3());
    public void RegisterProps(Stack<IProps> props);
    public int GetEmptyPointsCount();
   
    public IEnumerator FillingPoints();
    public Stack<IProps> GetTo(int amount);
}