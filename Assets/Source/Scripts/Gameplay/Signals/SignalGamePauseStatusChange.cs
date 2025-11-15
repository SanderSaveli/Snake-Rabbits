namespace SanderSaveli.Snake
{
    public readonly struct SignalGamePauseStatusChange
    {
        public readonly bool IsPause;

        public SignalGamePauseStatusChange(bool isPause)
        {
            IsPause = isPause;
        }
    }
}
