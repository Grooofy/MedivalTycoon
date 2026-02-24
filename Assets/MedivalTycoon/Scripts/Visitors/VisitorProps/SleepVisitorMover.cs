using Propses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepVisitorMover : MonoBehaviour, IPropsMover
{
    private IProps _sleepVisitor;
    public PropsType Type => PropsType.Visitor;

    public void RegistSleepVisitor(IProps sleepVisitor)
    {
        if (sleepVisitor != null)
            _sleepVisitor = sleepVisitor;
    }


    public Stack<IProps> GetTo(int amount)
    {
        var result = new Stack<IProps>();
       
        if(_sleepVisitor != null)
            result.Push(_sleepVisitor);
        
        return result;
    }

    public void RegisterProps(Stack<IProps> props) { }
    public void CreatePoints(int cout, float offset, Vector3 spaceSize = default) { }

    public IEnumerator FillingPoints()
    {
        throw new System.NotImplementedException();
    }

    public int GetEmptyPointsCount()
    {
        throw new System.NotImplementedException();
    }
}
