using SanderSaveli.UDK.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameScreenManager : ScreenManager<GameScreenType, GamePopupType>
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalInputOpenGameScreen>(HandleOpenScreen);
            _signalBus.Subscribe<SignalInputOpenGamePopup>(HandleOpenPopup);
            _signalBus.Subscribe<SignalInputClosePopup>(ClosePopup);
            _signalBus.Subscribe<SignalInputCloseScreen>(CloseOpenedWindow);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalInputOpenGameScreen>(HandleOpenScreen);
            _signalBus.Unsubscribe<SignalInputOpenGamePopup>(HandleOpenPopup);
            _signalBus.Unsubscribe<SignalInputClosePopup>(ClosePopup);
            _signalBus.Unsubscribe<SignalInputCloseScreen>(CloseOpenedWindow);
        }

        private void HandleOpenScreen(SignalInputOpenGameScreen ctx) =>
            OpenScreen(ctx.ScreenType);

        private void HandleOpenPopup(SignalInputOpenGamePopup ctx) =>
            AddToPopupQueue(ctx.PpopupType);
    }
}
