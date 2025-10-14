using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class PauseScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _resume;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exitToMenu;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _resume.onClick.AddListener(HandleResume);
            _restart.onClick.AddListener(HandleRestart);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _resume.onClick.AddListener(HandleResume);
            _restart.onClick.AddListener(HandleRestart);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleResume()
        {
            _signalBus.Fire(new SignalInputCloseScreen());
        }

        private void HandleRestart()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.RestartScene));
        }

        private void HandleExitToMenu()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }
    }
}
