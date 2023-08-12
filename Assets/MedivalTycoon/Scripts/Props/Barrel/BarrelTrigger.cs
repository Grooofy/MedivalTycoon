using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private Barrel barrel;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        
    }
}
