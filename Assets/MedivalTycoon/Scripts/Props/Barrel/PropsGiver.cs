using UnityEngine;

public class PropsGiver : MonoBehaviour, ITrigger
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
        if (hand.IsFull) return;

        hand.RegisterProps(_regulating);
        StartCoroutine(hand.FillingPoints());
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {
    }
}