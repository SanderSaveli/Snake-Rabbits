using UnityEngine;

namespace SanderSaveli.Snake
{
    public interface IScoreEffectShower
    {
        public void ShowAndAddScore(int score, Vector3 position);
    }
}
