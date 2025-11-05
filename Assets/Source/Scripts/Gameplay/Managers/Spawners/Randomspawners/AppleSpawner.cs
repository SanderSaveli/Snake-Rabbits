using SanderSaveli.UDK;
using SanderSaveli.UDK.Tools;
using System.Collections.Generic;
using Zenject;

namespace SanderSaveli.Snake
{
    public class AppleSpawner : RandomSpawner<Apple>
    {
        private ObjectPool<Apple> _applePool;
        private IGameField _gameField;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _applePool = new InjectionObjectPool<Apple>(_container, _objectPrefab, _parent);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalAppleEated>(SpawnApple);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalAppleEated>(SpawnApple);
        }

        public void SpawnApple()
        {
            SpawnRandom();
        }

        public override Apple Spawn(Cell cell)
        {
            Apple apple = _applePool.Get();
            apple.SetStartCell(cell);
            return apple;
        }
    }
}
