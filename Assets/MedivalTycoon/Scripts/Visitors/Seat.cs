using UnityEngine;

public class Seat : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform visitorPosition;
    [SerializeField] private GameObject occupiedIndicator;

    public bool IsOccupied { get; private set; }
    public Vector3 VisitorPosition => visitorPosition.position;

    public void Occupy(Visitor visitor)
    {
        IsOccupied = true;
        occupiedIndicator.SetActive(true);
        visitor.transform.position = VisitorPosition; 
    }

    public void Release()
    {
        IsOccupied = false;
        occupiedIndicator.SetActive(false);
    }
    
   
}