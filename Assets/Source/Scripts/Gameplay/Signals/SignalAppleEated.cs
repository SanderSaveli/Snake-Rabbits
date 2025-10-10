namespace SanderSaveli.Snake
{
    public readonly struct SignalAppleEated
    {
        public readonly Cell Cell;
        public readonly Apple Apple;

        public SignalAppleEated(Cell cell, Apple apple)
        {
            Cell = cell;
            Apple = apple;
        }
    }
}
