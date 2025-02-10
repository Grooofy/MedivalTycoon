using UnityEngine;

public class BarrelTaker : MonoBehaviour, ITrigger
{
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;


    private void OnEnable()
    {
        _regulating.Fulling += TurnCollider;
    }

    private void OnDisable()
    {
        _regulating.Fulling -= TurnCollider;
    }


    private void TurnCollider(bool value)
    {
        _collider.enabled = value;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand)) return;

        if (_regulating.IsMain)
        {
            hand.RegisterProps(_regulating);
            StartCoroutine(hand.FillingPoints());
        }

        if (!hand.IsFull) return;
        
        _regulating.RegisterProps(hand.GetTo());
        StartCoroutine(_regulating.FillingPoints());
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
        }
    }

    public void OnTriggerStay(Collider other)
    {
    }
}