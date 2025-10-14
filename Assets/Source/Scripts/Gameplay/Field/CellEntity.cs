using System;
using UnityEngine;
using Zenject;

public abstract class CellEntity : MonoBehaviour
{
    public Cell CurrentCell { get; protected set; }
    protected GameField GameField;

    [Inject]
    public void Construct(GameField gameField)
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
