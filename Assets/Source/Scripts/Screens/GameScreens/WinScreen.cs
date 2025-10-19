using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class WinScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _exitToMenu;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _nextLevel.onClick.AddListener(HandleNextLevel);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _nextLevel.onClick.RemoveListener(HandleNextLevel);
            _exitToMenu.onClick.RemoveListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleExitToMenu()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }

        private void HandleNextLevel()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Levels));
        }
    }
}
