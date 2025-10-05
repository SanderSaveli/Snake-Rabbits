namespace SanderSaveli.Snake
{
    public readonly struct SignalGameEnd
    {
        public readonly bool IsWin => Status == GameEndStatus.Win;
        public readonly GameEndStatus Status;

        public SignalGameEnd(GameEndStatus status)
        {
            Status = status;
        }
    }
}
