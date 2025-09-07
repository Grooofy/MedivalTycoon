using UnityEngine;

namespace Events
{
    public struct SeatPointClear
    {
        public Vector3 Position { get; private set; }

        public SeatPointClear(Vector3 position)
        {
            Position = position;
        }
    }
}