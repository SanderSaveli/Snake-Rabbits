using SanderSaveli.Snake;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SnakeHead : TickableCellEntity
{
    public Vector2Int HeadPosition => new Vector2Int(CurrentCell.X, CurrentCell.Y);
    public Direction Direction { get; set; }
    public Action<Cell> OnCellChange { get; set; }
    public Cell CurrentCell;

    [SerializeField] private SnakeMoveHandler _moveHandler;
    [SerializeField] private TailManager _tailManager;
    private SignalBus _signalBus;
    private bool _isNeedSpawnTail;

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

    public void AddLength()
    {
        _isNeedSpawnTail = true;
    }

    public override void SetStartCell(Cell cell)
    {
        CurrentCell = cell;
    }

    public void SetStartTailParts()
    {
        _tailManager.InitSnakeTail(this);
    }

    public override void Tick()
    {
        _tailManager.MoveTailParts();

        if (_isNeedSpawnTail)
        {
            _tailManager.AddTailPart();
            _isNeedSpawnTail = false;
        }

        Direction = _moveHandler.GetActualDirection();
        Vector2Int nextCellPos = HeadPosition + DirectionTool.DirectionToVector2(Direction);
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
        nextCell.SetEntity(this);
        CurrentCell = nextCell;
        OnCellChange?.Invoke(nextCell);
    }
}
