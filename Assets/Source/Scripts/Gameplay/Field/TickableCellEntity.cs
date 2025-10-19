using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class TickableCellEntity : CellEntity
    {
        [SerializeField] protected TickOrder TickOrder;
        protected float TickTime {  get; private set; }
        protected GameLoop _gameLoop;

        [Inject]
        public void Construct(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;
        }

        private void OnEnable()
        {
            _gameLoop.SubscribeToTick(this, (int)TickOrder);
            TickTime = _gameLoop.TickTime;
        }

        private void OnDisable()
        {
            _gameLoop.UnsubscribeFromTick(this);
        }

        protected void RemoveFromField()
        {
            _gameLoop.UnsubscribeFromTick(this);
            CurrentCell.SetEntity(null);
        }

        public abstract void Tick();
    }
}
