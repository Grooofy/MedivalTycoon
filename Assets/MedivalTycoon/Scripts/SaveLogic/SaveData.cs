using System;

[Serializable]
public class SaveData
{
    public float GuestWaitSeconds = 120f;
    public int MinBeerAmount = 3;
    public int MaxBeerAmount = 4;
    public int NumberLevel;
    public int StartMoney; 
    public int NumberVisitors;
    public float Seconds;
    public int TableAmount;
    public string TableCost;
    public bool IsTutorialCompleted;
}