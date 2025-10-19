using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class Rabbit : TickableCellEntity
    {
        public Direction LookAt { get; private set; }
        [SerializeField] private RabbitAI _rabbitAI;
        public Action<Cell> OnCellChange;
        public Action<Direction> OnRotate;
        public Action OnKill;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        public override void CollideWithHead(SnakeHead snake, out bool isKillSnake)
        {
            isKillSnake = false;
            snake.AddLength();
            Die();
        }

        public override Type GetEntityType() => typeof(Rabbit);

        public override void Tick()
        {
            _rabbitAI.Tick();
        }

        public void Rotate(Direction direction)
        {
            LookAt = direction;
            OnRotate?.Invoke(direction);
        }

        public void MoveForward()
        {
            Cell nextCell = GetFowardCell();

            if (nextCell == null || nextCell.IsOccupied)
            {
                Debug.Log("Cell is IsOccupied");
                return;
            }

            ChangeCell(nextCell);
            OnCellChange?.Invoke(nextCell);
        }

        public Cell GetFowardCell()
        {
            Vector2Int nextCellPos = CurrentCell.Position + DirectionTool.DirectionToVector2(LookAt);
            if (!GameField.IsInBounds(nextCellPos))
            {
                return null;
            }

            Cell nextCell = GameField[nextCellPos.x, nextCellPos.y];
            return nextCell;
        }

        public void Die()
        {
            Debug.Log("Rabbit die");
            RemoveFromField();
            _signalBus.Fire(new SignalRabbitEated(this));
            OnKill?.Invoke();
        }
    }
}
