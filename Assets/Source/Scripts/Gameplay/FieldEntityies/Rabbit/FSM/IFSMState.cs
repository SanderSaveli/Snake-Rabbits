namespace SanderSaveli.Snake
{
    public interface IFSMState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();

    }
}
