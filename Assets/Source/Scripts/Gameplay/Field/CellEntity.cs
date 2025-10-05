using System;
using UnityEngine;
using Zenject;

public abstract class CellEntity : MonoBehaviour
{
    protected GameField GameField;

    [Inject]
    public void Construct(GameField gameField)
    {
        GameField = gameField;
    }

    public Type EntityType => GetEntityType();

    public abstract void SetStartCell(Cell cell);

    public abstract void CollideWithHead(SnakeHead snake, out bool IsKillSnake);
    public abstract Type GetEntityType();
}
