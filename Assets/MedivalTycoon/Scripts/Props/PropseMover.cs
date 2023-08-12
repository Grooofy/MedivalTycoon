using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PropseMover : MonoBehaviour
{
    

    private void MovedTo(Vector3 positionEndPoint, Barrel item)
    {
        item.transform.position =
               Vector3.MoveTowards(item.transform.position, positionEndPoint, Time.deltaTime);
    }

}
