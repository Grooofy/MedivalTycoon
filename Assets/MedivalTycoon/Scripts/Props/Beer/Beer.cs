using System.Collections;
using System.Collections.Generic;
using Propses;
using UnityEngine;

public class Beer : MonoBehaviour, IProps
{
    public GameObject Prefab { get; }
    
    
    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator TryMoveTo(Point endPoints)
    {
        throw new System.NotImplementedException();
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        throw new System.NotImplementedException();
    }
}
