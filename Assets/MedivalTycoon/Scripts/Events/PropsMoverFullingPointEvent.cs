public struct PropsMoverFullingPointEvent
{
    public bool IsFull;
    public string SourceId;

    public PropsMoverFullingPointEvent(bool isFull,  string sourceId)
    {
        SourceId = sourceId;
        IsFull = isFull;
    }
}