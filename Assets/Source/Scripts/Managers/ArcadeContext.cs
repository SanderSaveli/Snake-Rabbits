using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ArcadeContext : DoNotDestroyOnLoadSingletone<ArcadeContext>
    {
        public int Score { get; set; }
        public int LevelsComplete { get; set; }
        public int Health { get; set; }
        private bool _isFirstRestart = true;
        public override void Awake()
        {
            base.Awake();
            if(Instance == this && _isFirstRestart)
            {
                ResetContext();
                _isFirstRestart = false;
            }
        }

        public void ResetContext()
        {
            Score = 0;
            LevelsComplete = 0;
            Health = 3;
            Debug.Log("Arcade Context Reset");
        }
    }
}
