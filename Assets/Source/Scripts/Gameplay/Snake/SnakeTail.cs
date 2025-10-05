using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class SnakeTail : CellEntity
    {
        public Action<Cell> OnChangeCell;
        public Cell CurrentCell;

        public void ChangeCell(Cell cell)
        {
            CurrentCell = cell;
            cell.SetEntity(this);
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

        public override void SetStartCell(Cell cell)
        {
            CurrentCell = cell;
        }
    }
}
