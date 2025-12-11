using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ScoreManager : MonoBehaviour, IScoreManager
    {
        public int Score { get; protected set; }

        protected SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalAddScore>(HandleAddScore);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalAddScore>(HandleAddScore);
        }

        protected void HandleAddScore(SignalAddScore ctx)
        {
            Score += ctx.Score;
            _signalBus.Fire(new SignalScoreChanged(Score, ctx.Score));
        }
    }
}
