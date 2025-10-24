using SanderSaveli.UDK.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class StartFade : UiScreen
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            ShowImmediately();
        }

        protected override void SubscribeToEvents()
        {
            _signalBus.Subscribe<SignalGameStart>(HandleGameStart);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _signalBus.Unsubscribe<SignalGameStart>(HandleGameStart);
            base.UnsubscribeFromEvents();
        }

        private void HandleGameStart()
        {
            Hide();
        }
    }
}
