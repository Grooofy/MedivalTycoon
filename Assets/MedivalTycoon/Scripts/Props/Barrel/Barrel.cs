using System.Collections;
using UnityEngine;

public class Barrel : Props
{
    internal override IEnumerator TryMoveTo(Transform endPoint)
    {
        if (endPoint != null)
        {
            while (IsMinDistance(transform.position, endPoint.position) == false)
            {
                MoveTo(endPoint);
                yield return null;
            }
        }
    }
}
