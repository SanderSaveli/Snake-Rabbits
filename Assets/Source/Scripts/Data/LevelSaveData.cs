using System;

namespace SanderSaveli.Snake
{
    [Serializable]
    public struct LevelSaveData
    {
        public int level_number;
        public int max_score;
        public float best_time;
        public bool is_complete => star_count > 0;
        public int star_count;

        public LevelSaveData(int levelNumber, int maxScore, float bestTime, int starCount)
        {
            level_number = levelNumber;
            max_score = maxScore;
            best_time = bestTime;
            star_count = starCount;
        }
    }
}
