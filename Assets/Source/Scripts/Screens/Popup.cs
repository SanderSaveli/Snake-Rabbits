using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class Popup : UiScreen
    {
        [SerializeField] private Button _closeButton;

        protected SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _closeButton.onClick.AddListener(HandleClose);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _closeButton.onClick.RemoveListener(HandleClose);
        }

        private void HandleClose()
        {
            _signalBus.Fire(new SignalInputClosePopup());
        }
    }
}
