namespace SanderSaveli.Snake
{
    public readonly struct SignalInputOpenGameScreen
    {
        public readonly GameScreenType ScreenType;

        public SignalInputOpenGameScreen(GameScreenType ScreenType)
        {
            this.ScreenType = ScreenType;
        }
    }
}
