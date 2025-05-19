using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : Props
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
