using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "CreateLevel", order = 42)]
public class Level : ScriptableObject
{
    [SerializeField] private int _numberLevel;
    [SerializeField] private int _startMoney; 
    [SerializeField] private int _numberVisitors;
    [SerializeField] private float _seconds;
    [SerializeField] private bool _isPassed;

    public int NumberLevel => _numberLevel;
    public int StartMoney => _startMoney;
    public int NumberVisitors => _numberVisitors;
    public float Seconds => _seconds;
}
