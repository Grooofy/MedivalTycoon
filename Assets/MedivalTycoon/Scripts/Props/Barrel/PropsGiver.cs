using Characters;
using UnityEngine;

public class PropsGiver : MonoBehaviour, ITrigger
{
    [SerializeField] private SphereCollider _collider;
    private IPropsMover _regulating;

    /*private void OnEnable()
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
    }*/


    public void Initialize(IPropsMover regulating)
    {
        _regulating = regulating;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand)) return;
       
        Debug.Log(hand.name + " is full");
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