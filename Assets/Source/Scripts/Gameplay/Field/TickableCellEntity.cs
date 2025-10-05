using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class TickableCellEntity : CellEntity
    {
        [SerializeField] protected int TickOrder;
        protected float TickTime {  get; private set; }
        protected GameLoop _gameLoop;

        [Inject]
        public void Construct(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;
        }

        private void OnEnable()
        {
            _gameLoop.SubscribeToTick(this, TickOrder);
            TickTime = _gameLoop.TickTime;
        }

        private void OnDisable()
        {
            _gameLoop.UnsubscribeFromTick(this);
        }

        public abstract void Tick();
    }
}
