using Propses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepVisitorBuffer : MonoBehaviour, IPropsMover
{
    private Stack<IProps> _props = new Stack<IProps>();
    private Point _finishPoint; 
    public PropsType Type => PropsType.Visitor;

    internal void Initialize(Point finishPoint)
    {
        _finishPoint = finishPoint;
    }

    public void RegisterProps(Stack<IProps> props)
    {
        if (props == null) return;
        if (props.Count == 0) return;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            _props.Push(prop);
        }
    }

    public IEnumerator FillingPoints()
    {
        while (_props.Count > 0)
        {
            _props.TryPop(out var props);
            if (props == null) break;

            StartCoroutine(props.TryMoveTo(_finishPoint));               
            yield return WaitFor.QuarterSecond;
            props.Reset();
            _finishPoint.Free();
        }
    }

    public void CreatePoints(int cout, float offset, Vector3 spaceSize = default)
    {
        throw new System.NotImplementedException();
    }

    public int GetEmptyPointsCount()
    {
        throw new System.NotImplementedException();
    }

    public Stack<IProps> GetTo(int amount)
    {
        throw new System.NotImplementedException();
    }

    
}
