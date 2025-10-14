using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class MenuScreen : UiScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _playButton.onClick.AddListener(HandlePlay);
            _exitButton.onClick.AddListener(HandleExit);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _playButton.onClick.RemoveListener(HandlePlay);
            _exitButton.onClick.RemoveListener(HandleExit);
            base.UnsubscribeFromEvents();
        }

        private void HandlePlay()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Levels));
        }

        private void HandleExit()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.ExitGame));
        }
    }
}
