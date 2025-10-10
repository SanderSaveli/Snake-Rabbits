namespace SanderSaveli.Snake
{
    public readonly struct SignalScoreChanged
    {
        public readonly int CurrentScore;
        public readonly int ScoreDelta;

        public SignalScoreChanged(int currentScore, int scoreDelta)
        {
            CurrentScore = currentScore;
            ScoreDelta = scoreDelta;
        }
    }
}
