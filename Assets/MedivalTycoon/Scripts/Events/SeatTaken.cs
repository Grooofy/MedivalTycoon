namespace Events
{
    public struct SeatTaken
    {
        public Seat Seat { get; private set; }

        public SeatTaken(Seat seat)
        {
            Seat = seat;
        }
    }
}