using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ArcadeGameEndHandler : GameEndHandler
    {
        private IHealthManager _healthManager;
        private IScoreManager _scoreManager;

        [Inject]
        public void Construct(IHealthManager healthManager, IScoreManager scoreManager)
        {
            _healthManager = healthManager;
            _scoreManager = scoreManager;
        }

        public override void Lose()
        {
            ArcadeContext context = ArcadeContext.Instance;
            _healthManager.Health--;
            context.Health = _healthManager.Health;
            if (context.Health > 0)
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.ArcadeRestartLose));
                Debug.Log("Arcade lose");
            }
            else
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.ArcadeEnd));
                context.ResetContext();
                Debug.Log("Arcade End");
            }
        }

        public override void Win()
        {
            ArcadeContext context = ArcadeContext.Instance;
            context.Score += _scoreManager.Score;
            context.LevelsComplete++;
            _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.ArcadeRestartWin));
        }
    }
}
