using System.Collections;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform _sitPosition;
    [SerializeField] private GameObject _orderDisplay;
    
    private Visitor _currentVisitor;
    public bool IsOccupied => _currentVisitor != null;

    public void AssignVisitor(Visitor visitor)
    {
        _currentVisitor = visitor;
        visitor.MoveToSeat(_sitPosition.position);
        StartCoroutine(OrderProcess(visitor));
    }

    private IEnumerator OrderProcess(Visitor visitor)
    {
        // Активация отображения заказа
        _orderDisplay.SetActive(true);
        _orderDisplay.GetComponentInChildren<TMPro.TextMeshPro>().text = 
            visitor.RequiredOrders.ToString();

        // Таймер приготовления
        float timer = 0;
        while(timer < visitor.PreparationTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        visitor.CompleteOrder();
        _orderDisplay.SetActive(false);
    }

    public void ReleaseSeat()
    {
        _currentVisitor = null;
    }
}