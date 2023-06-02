using UnityEngine;

public class Barrel : Prop
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BarrelSpot _))
        {
            StopAnimation();
        }
    }
}
