using System;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class Rabbit : TickableCellEntity
    {
        public Direction LookAt { get; private set; }
        [SerializeField] private RabbitAI _rabbitAI;
        public Action<Cell> OnCellChange;

        public override void CollideWithHead(SnakeHead snake, out bool isKillSnake)
        {
            isKillSnake = false;
        }

        public override Type GetEntityType() => typeof(Rabbit);

        public override void Tick()
        {
            _rabbitAI.Tick();
        }

        public void Rotate(Direction direction)
        {
            LookAt = direction;
        }

        public void MoveForward()
        {
            Cell nextCell = GetFowardCell();

            if (nextCell == null || nextCell.IsOccupied)
            {
                return;
            }

            nextCell.SetEntity(this);
            CurrentCell = nextCell;
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
    }
}
