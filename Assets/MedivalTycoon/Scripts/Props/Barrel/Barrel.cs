using System.Collections;
using UnityEngine;

public class Barrel : Props
{
    internal override IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint.IsFill == false)
        {
            while (IsMinDistance(transform.position, endPoint.transform.position) == false)
            {
                MoveTo(endPoint);
                yield return null;
            }
        }
    }
}
