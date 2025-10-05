namespace SanderSaveli.Snake
{
    public readonly struct SignalInputChangeDirection
    {
        public readonly Direction Direction;

        public SignalInputChangeDirection(Direction direction)
        {
            Direction = direction;
        }
    }
}
