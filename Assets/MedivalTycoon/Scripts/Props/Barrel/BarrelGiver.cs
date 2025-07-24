using Characters;
using UnityEngine;

public class BarrelGiver : MonoBehaviour, ITrigger
{
    private IPropsMover _regulating;

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