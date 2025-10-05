using SanderSaveli.Snake;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SnakeHead : TickableCellEntity
{
    public Vector2Int HeadPosition => new Vector2Int(_currentCell.X, _currentCell.Y);
    public Direction Direction { get; set; }
    private Cell _currentCell;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public override void CollideWithHead(SnakeHead snake, out bool IsKillSnake)
    {
        IsKillSnake = true;
        snake.Die();
    }

    public override Type GetEntityType() => typeof(SnakeHead);

    public void Die()
    {
        Time.timeScale = 0;
        Debug.Log("Snake Die!");
        _signalBus.Fire(new SignalGameEnd(GameEndStatus.Lose_collide));
    }

    public override void SetStartCell(Cell cell)
    {
        _currentCell = cell;
        transform.position = GameField.CellToWorld(cell.X, cell.Y) + new Vector3(0, 0, -1);
    }

    public override void Tick()
    {
        Vector2Int nextCellPos = HeadPosition + DirectionToVector2(Direction);
        if(!GameField.IsInBounds(nextCellPos))
        {
            Die();
            return;
        }

        Cell nextCell = GameField[nextCellPos.x, nextCellPos.y];

        if(nextCell.IsOccupied)
        {
            nextCell.Entity.CollideWithHead(this, out bool isKill);
            if(isKill)
            {
                return;
            }
        }

        _currentCell.SetEntity(null);
        nextCell.SetEntity(this);
        _currentCell = nextCell;

        transform.position = GameField.CellToWorld(nextCellPos) + new Vector3(0, 0, -1);
    }

    public Vector2Int DirectionToVector2(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Right:
                return Vector2Int.right;
            default:
                throw new NotImplementedException($"There is no case for Direction {direction}");
        }
    }
}
