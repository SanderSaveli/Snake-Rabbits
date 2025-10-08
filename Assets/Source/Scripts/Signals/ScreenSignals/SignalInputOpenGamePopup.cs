namespace SanderSaveli.Snake
{
    public readonly struct SignalInputOpenGamePopup
    {
        public readonly GamePopupType PpopupType;

        public SignalInputOpenGamePopup(GamePopupType popupType)
        {
            PpopupType = popupType;
        }
    }
}
