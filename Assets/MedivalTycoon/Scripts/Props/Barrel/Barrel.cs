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

    internal override IEnumerator TryJumpTo(Point endPoint, Vector3 startPosition, float elapsedTime, float moveDuration)
    {
        while (elapsedTime < moveDuration)
        {
           JumpTo(endPoint, transform.position, elapsedTime, moveDuration);
           yield return null;
        }
        transform.position = endPoint.transform.position;
    }
}
