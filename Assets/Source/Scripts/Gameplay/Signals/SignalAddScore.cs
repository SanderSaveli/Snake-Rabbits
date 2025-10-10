namespace SanderSaveli.Snake
{
    public readonly struct SignalAddScore
    {
        public readonly int Score;

        public SignalAddScore(int score)
        {
            Score = score;
        }
    }
}
