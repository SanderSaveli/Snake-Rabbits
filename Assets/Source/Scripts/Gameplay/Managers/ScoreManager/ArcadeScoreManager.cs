namespace SanderSaveli.Snake
{
    public class ArcadeScoreManager : ScoreManager
    {
        private void Start()
        {
            ArcadeContext context = ArcadeContext.Instance;
            Score = context.Score;
            _signalBus.Fire(new SignalScoreChanged(Score, Score));
        }
    }
}