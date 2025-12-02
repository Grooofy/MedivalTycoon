public struct PropsMoverFullingPointEvent
{
    public bool IsFull { get; private set; }
    public string SourceId { get; private set; }

    public PropsMoverFullingPointEvent(bool isFull,  string sourceId)
    {
        SourceId = sourceId;
        IsFull = isFull;
    }
}