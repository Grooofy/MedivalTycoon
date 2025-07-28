using Characters;
using UnityEngine;

public class BarrelTaker : MonoBehaviour, ITrigger
{
    private IPropsMover _regulating;
    
    public void Initialize(IPropsMover regulating)
    {
        _regulating = regulating;
    }
    
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand)) return;

        var props = hand.GetTo(hand.Amount); 

        if (props == null || props.Count == 0)
        {
            Debug.LogWarning("No props to transfer from Hand to Regulating.");
            return;
        }

        _regulating.RegisterProps(props);
        StartCoroutine(_regulating.FillingPoints());
    }

    public void OnTriggerStay(Collider other)
    {
    }

    public void OnTriggerExit(Collider other)
    {
    }
}