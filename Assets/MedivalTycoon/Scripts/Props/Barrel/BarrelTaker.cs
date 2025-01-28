using UnityEngine;

public class BarrelTaker : MonoBehaviour, ITrigger
{
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;


    private void OnEnable()
    {
        //_regulating.Fulling += TurnCollider;
    }

    private void OnDisable()
    {
       // _regulating.Fulling -= TurnCollider;
    }

    
    private void TurnCollider(bool value)
    {
        _collider.enabled = value;
    }

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
