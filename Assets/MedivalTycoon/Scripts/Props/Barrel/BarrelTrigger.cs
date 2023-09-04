using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;

    private GameObject _takenBarrel;

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            
        }
    }
}
