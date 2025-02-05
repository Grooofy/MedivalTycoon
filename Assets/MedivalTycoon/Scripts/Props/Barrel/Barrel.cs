using System.Collections;
using UnityEngine;

public class Barrel : Props
{ 
    internal override IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint.IsFill == false && endPoint != null)
        {
            while (IsMinDistance(transform.position, endPoint.transform.position) == false)
            {
                MoveTo(endPoint);
                yield return null;
            }
            transform.position = endPoint.transform.position;
            
        }
    }

    internal override IEnumerator TryJumpTo(Point endPoint, float elapsedTime, float moveDuration)
    {
        Vector3 startPosition = transform.position;
        
        while (elapsedTime < moveDuration)
        {
           JumpTo(endPoint, startPosition, elapsedTime, moveDuration);
           yield return null;
        }
        transform.position = endPoint.transform.position;
    }
}
