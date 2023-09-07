using UnityEngine;

public class BarrelTaker : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            bartender.ReceiveObject(_regulating);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            bartender.Stop();
        }
    }

    public void OnTriggerStay(Collider other)
    {
       
    }
}
