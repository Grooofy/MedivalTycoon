using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropsMover
{
    public void CreatePoints(int cout, float offset);
    public void RegisterProps(Queue<Props> props);
    public void RegisterProp(Props props);
    public Action<bool> Fulling { get; set; }
    public IEnumerator FillingPoints();
    public Queue<Props> GetTo(int amount);
}