namespace Events
{
    public struct BeerBufferOpen
    {
        public bool IsEmpty { get; private set; }

        public BeerBufferOpen(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }
    }
}