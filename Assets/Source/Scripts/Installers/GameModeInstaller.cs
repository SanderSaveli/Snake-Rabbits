using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class GameModeInstaller : MonoInstaller
    {
        protected DiContainer _container;
        public void SetContainer(DiContainer container)
        {
            _container = container;
        }
    }
}
