using UnityEngine;

public class PropsTaker : MonoBehaviour, ITrigger
{
    [SerializeField] private Regulating _regulating;

    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand)) return;

        var props = hand.GetTo(3); // Получаем до 3 объектов из Hand

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