using UnityEngine;

public class Barrel : Props
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BarrelSpot _))
        {
            StopRotationAnimation();
        }
    }
}
