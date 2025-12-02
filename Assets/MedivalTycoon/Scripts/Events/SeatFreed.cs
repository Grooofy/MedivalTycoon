namespace Events
{
    public struct SeatFreed
    {
        public Seat Seat { get; private set; }

        public SeatFreed(Seat seat)
        {
            Seat = seat;
        }
    }
}