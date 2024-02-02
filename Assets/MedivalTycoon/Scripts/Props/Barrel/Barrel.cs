using System.Collections;
using UnityEngine;

public class Barrel : Props
{
    internal override IEnumerator TryMoveTo(Transform endPoint)
    {
         MoveTo(endPoint);
         yield return new WaitForSeconds(1);
    }
}