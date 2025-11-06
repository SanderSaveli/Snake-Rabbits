using System;

namespace SanderSaveli.Snake
{
    [Serializable]
    public struct LevelSaveData
    {
        public int max_score;
        public float best_time;

        public LevelSaveData(int maxScore, float bestTime)
        {
            max_score = maxScore;
            best_time = bestTime;
        }
    }
}
