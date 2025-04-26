using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager; // Менеджер очереди
    private List<Seat> seats = new List<Seat>();       // Список всех мест

    

    // Добавить место
    public void AddSeat(Seat seat)
    {
        if (!seats.Contains(seat))
        {
            seats.Add(seat);
            seat.OnSeatVacated += HandleSeatVacated; // Подписываемся на событие освобождения места
            Debug.Log("Место добавлено.");
        }
    }

    // Удалить место
    public void RemoveSeat(Seat seat)
    {
        if (seats.Contains(seat))
        {
            seats.Remove(seat);
            seat.OnSeatVacated -= HandleSeatVacated; // Отписываемся от события
            Debug.Log("Место удалено.");
        }
    }

    // Обработка освобождения места
    private void HandleSeatVacated(Seat seat)
    {
        Debug.Log("Место освобождено.");
        queueManager.AssignSeatToNextGuest(seat); // Сообщаем менеджеру очереди о свободном месте
    }
}