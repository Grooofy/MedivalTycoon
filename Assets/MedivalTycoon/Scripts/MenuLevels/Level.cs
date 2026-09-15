using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "CreateLevel", order = 42)]
public class Level : ScriptableObject
{
    [SerializeField] private int _numberLevel;
    [SerializeField] private int _startMoney; 
    [SerializeField] private int _numberVisitors;
    [SerializeField] private float _seconds;
    [SerializeField] private bool _isComplete;
    [SerializeField] private int _tableAmount;
    [SerializeField] private string _tableCost;

    [Header("Guest orders")]
    [SerializeField, Min(0.1f)] private float _guestWaitSeconds = 120f;
    [SerializeField, Min(1)] private int _minBeerAmount = 3;
    [SerializeField, Min(1)] private int _maxBeerAmount = 4;
    public float GuestWaitSeconds => Mathf.Max(0.1f, _guestWaitSeconds);
    public int MinBeerAmount => Mathf.Clamp(_minBeerAmount, 1, int.MaxValue - 1);
    public int MaxBeerAmount => Mathf.Clamp(_maxBeerAmount, MinBeerAmount, int.MaxValue - 1);

    public int NumberLevel => _numberLevel;
    public int StartMoney => _startMoney;
    public int NumberVisitors => _numberVisitors;
    public float Seconds => _seconds;

    public bool IsComplete => _isComplete;
    
    public int TableAmount => _tableAmount;
    public string TableCost => _tableCost;
}
