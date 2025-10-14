namespace SanderSaveli.Snake
{
    public readonly struct SignalInputAction
    {
        public readonly InputActionType ActionType;

        public SignalInputAction(InputActionType actionType)
        {
            ActionType = actionType;
        }
    }
}
