namespace Events
{
    public struct TableBuilt
    {
        public Seat SeatPoint {get; private set;}

        public TableBuilt(Seat seatPoint)
        {
            SeatPoint = seatPoint;
        }
    }
}