using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Barrel : Props
{
    internal override IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint == null) yield break;
        
        while (endPoint.IsFill == false)
        {
            MoveTo(endPoint);
            yield return null;
        }
        transform.position = endPoint.transform.position;
    }
    
}
