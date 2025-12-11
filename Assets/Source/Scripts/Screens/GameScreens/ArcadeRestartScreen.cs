using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class ArcadeRestartScreen : TimeStopScreen
    {
        [SerializeField] private Button _continueButton;

        protected override void SubscribeToEvents()
        {
            _continueButton.onClick.AddListener(HandleContinue);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _continueButton.onClick.RemoveListener(HandleContinue);
            base.UnsubscribeFromEvents();
        }

        private void HandleContinue()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Arcade));
        }
    }
}
