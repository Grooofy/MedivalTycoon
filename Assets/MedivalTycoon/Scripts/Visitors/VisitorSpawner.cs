using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisitorSpawner : MonoBehaviour
{
    [Header("Основные настройки")] [SerializeField]
    private Visitor _visitorPrefab; // Префаб посетителя

    [SerializeField] private Transform _spawnPoint; // Точка появления
    [SerializeField] private Transform _queueStart; // Начало очереди
    [SerializeField] private float _queueSpacing = 1.5f; // Расстояние между NPC

    [Header("Параметры спавна")] [SerializeField]
    private int _initialSpawn = 5; // Посетителей при старте

    [SerializeField] private float _spawnInterval = 10f; // Интервал между спавном
    [SerializeField] private int _maxVisitors = 15; // Макс. длина очереди

    private QueueManager _queueManager;
    private List<Vector3> _queuePositions = new List<Vector3>();

    private void Awake()
    {
        _queueManager = FindObjectOfType<QueueManager>();
        CalculateQueuePositions();
        InitialSpawn();
        StartCoroutine(AutoSpawn());
    }

    // Рассчитать позиции в очереди
    private void CalculateQueuePositions()
    {
        _queuePositions.Clear();
        for (int i = 0; i < _maxVisitors; i++)
        {
            Vector3 pos = _queueStart.position +
                          new Vector3(_queueSpacing, 0, 0);
            _queuePositions.Add(pos);
        }
    }

    // Первоначальный спавн
    private void InitialSpawn()
    {
        for (int i = 0; i < _initialSpawn; i++)
        {
            SpawnVisitor();
        }
    }

    // Автоматический спавн
    private IEnumerator AutoSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            if (_queueManager.QueueCount < _maxVisitors)
            {
                SpawnVisitor();
            }
        }
    }

    // Создание одного посетителя
    public void SpawnVisitor()
    {
        Visitor newVisitor = Instantiate(
            _visitorPrefab,
            _spawnPoint.position,
            Quaternion.identity
        );

        // Инициализация параметров
        newVisitor.Initialize(
            orders: Random.Range(1, 5),
            prepTime: Random.Range(8f, 15f)
        );

        // Добавление в очередь
        _queueManager.AddVisitorToQueue(newVisitor);
        UpdateAllPositions();
    }

    // Обновить позиции всех в очереди
    private void UpdateAllPositions()
    {
        int index = 0;
        foreach (Visitor visitor in _queueManager.GetAllVisitors())
        {
            if (index >= _queuePositions.Count) break;
            visitor.MoveToPosition(_queuePositions[index]);
            index++;
        }
    }
}