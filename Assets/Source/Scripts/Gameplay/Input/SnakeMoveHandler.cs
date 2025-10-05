using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SnakeMoveHandler : MonoBehaviour
    {
        [SerializeField] private SnakeHead _snakeHead;
        private Direction _direction;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _direction = _snakeHead.Direction;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalInputChangeDirection>(HandleChangeDirection);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalInputChangeDirection>(HandleChangeDirection);
        }

        public Direction GetActualDirection()
        {
            return _direction;
        }

        private void HandleChangeDirection(SignalInputChangeDirection ctx)
        {
            if (DirectionTool.IsSide(_snakeHead.Direction, ctx.Direction))
            {
                _direction = ctx.Direction;
            }
        }
    }
}
