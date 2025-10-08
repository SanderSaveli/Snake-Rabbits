using SanderSaveli.UDK;
using System;

namespace SanderSaveli.Snake
{
    public class Apple : CellEntity, IPoolableObject<Apple>
    {
        public Cell CurrentCell;
        public Action<Apple> OnBackToPool { get; set; }

        public override void CollideWithHead(SnakeHead snake, out bool isKillSnake)
        {
            isKillSnake = false;
            snake.AddLength();
            OnBackToPool?.Invoke(this);
        }

        public override Type GetEntityType()
        {
            return typeof(Apple);
        }

        public void OnActive()
        {

        }

        public override void SetStartCell(Cell cell)
        {
            CurrentCell = cell;
            transform.position = cell.WorldPosition;
        }
    }
}
