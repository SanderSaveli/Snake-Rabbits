using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class CellEntity : MonoBehaviour
    {
        public Cell CurrentCell { get; protected set; }
        protected IGameField GameField;

        [Inject]
        public void Construct(IGameField gameField)
        {
            GameField = gameField;
        }

        public Type EntityType => GetEntityType();

        public virtual void SetStartCell(Cell cell)
        {
            CurrentCell = cell;
            cell.SetEntity(this);
            transform.position = cell.WorldPosition;
        }

        public abstract void CollideWithHead(SnakeHead snake, out bool isKillSnake);
        public abstract Type GetEntityType();
    }

}