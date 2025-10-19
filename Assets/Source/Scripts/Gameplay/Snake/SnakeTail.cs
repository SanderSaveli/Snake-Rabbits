using System;

namespace SanderSaveli.Snake
{
    public class SnakeTail : CellEntity
    {
        public Action<Cell> OnChangeCell;

        public new void ChangeCell(Cell cell)
        {
            base.ChangeCell(cell);
            OnChangeCell?.Invoke(cell);
        }

        public override void CollideWithHead(SnakeHead snake, out bool IsKillSnake)
        {
            IsKillSnake = true;
            snake.Die();
        }

        public override Type GetEntityType()
        {
            return typeof(SnakeTail);
        }
    }
}
