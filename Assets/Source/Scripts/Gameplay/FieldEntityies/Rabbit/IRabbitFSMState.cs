namespace SanderSaveli.Snake
{
    public interface IRabbitState : IFSMState
    {
        void Initialize(Rabbit rabbit, FSM<IRabbitState> fsm);
    }
}
