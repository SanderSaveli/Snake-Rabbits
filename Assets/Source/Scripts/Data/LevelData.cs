namespace SanderSaveli.Snake
{
    public class LevelData
    {
        public int MaxScore { get; private set; }
        public int Stars { get; private set; }
        public int ScoreForFirstStar { get; private set; }
        public int ScoreForSecondStar { get; private set; }
        public int ScoreForThirdStar { get; private set; }
        public bool IsPassed { get; private set; }
        public int LevelNumber { get; private set; }

        public LevelData(LevelConfig config, LevelSaveData data)
        {
            MaxScore = data.max_score;
            LevelNumber = config.level_number;

            ScoreForFirstStar = config.score_for_first_star;
            ScoreForSecondStar = config.score_for_second_star;
            ScoreForThirdStar = config.score_for_third_star;

            if (MaxScore >= config.score_for_third_star)
            {
                Stars = 3;
                IsPassed = true;
            }
            else if (MaxScore >= config.score_for_second_star)
            {
                Stars = 2;
                IsPassed = true;
            }
            else if (MaxScore >= config.score_for_first_star)
            {
                Stars = 1;
                IsPassed = true;
            }
            else
            {
                Stars = 0;
                IsPassed = false;
            }
        }
    }
}
