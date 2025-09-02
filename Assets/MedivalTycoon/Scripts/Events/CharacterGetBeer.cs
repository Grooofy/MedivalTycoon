namespace Events
{
    public struct CharacterGetBeer
    {
        public bool IsFull { get; private set; }

        public CharacterGetBeer(bool isFull)
        {
            IsFull = isFull;
        }
    }
}