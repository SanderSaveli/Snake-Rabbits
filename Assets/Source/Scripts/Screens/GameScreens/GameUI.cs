using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameUI : MonoBehaviour
    {
        public bool IsActive => !_isOnPause && !_isGameEnd;

        [Header("Buttons")]
        [SerializeField] private Button _pause;

        private SignalBus _signalBus;
        private bool _isOnPause;
        private bool _isGameEnd;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _pause.onClick.AddListener(HandleOpenPause);
            _signalBus.Subscribe<SignalGamePauseStatusChange>(HandleGamePauseStatusChange);
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _pause.onClick.RemoveListener(HandleOpenPause);
            _signalBus.Unsubscribe<SignalGamePauseStatusChange>(HandleGamePauseStatusChange);
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void HandleOpenPause()
        {
            if(IsActive)
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Pause));
            }
        }

        private void HandleGameEnd()
        {
            _isGameEnd = true;
        }

        private void HandleGamePauseStatusChange(SignalGamePauseStatusChange ctx)
        {
            _isOnPause = ctx.IsPause;
        }
    }
}
