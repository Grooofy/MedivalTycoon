public struct PropsMoverFullingPointEvent
{
    public bool IsFull { get; private set; }

    public PropsMoverFullingPointEvent(bool isFull)
    {
        IsFull = isFull;
    }
}