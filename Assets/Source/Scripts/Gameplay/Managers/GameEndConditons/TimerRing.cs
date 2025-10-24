using SanderSaveli.UDK;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class TimerRing : MonoBehaviour
    {
        [SerializeField] private GameTimerView _timerView;

        private SignalBus _signalBus;
        private float _gameDuration;
        private TimerHandle _timerHandle;

        [Inject]
        public void Construct(SignalBus signalBus, GameplayConfig gameplayConfig)
        {
            _signalBus = signalBus;
            _gameDuration = gameplayConfig.GameDuration;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameStart>(HandleGameStart);
            _timerView.UpdateView(1);
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameStart>(HandleGameStart);
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void HandleGameStart()
        {
            _timerHandle = Timer.StartTimer(_gameDuration, HandleTimerRing, HandleTimerUpdate);
        }

        private void HandleTimerRing()
        {
            _signalBus.Fire(new SignalGameEnd(GameEndStatus.Lose_time_end));
        }

        private void HandleGameEnd()
        {
            _timerHandle.Cancel();
        }

        private void HandleTimerUpdate(float amount)
        {
            _timerView.UpdateView(amount / _gameDuration);
        }
    }
}
