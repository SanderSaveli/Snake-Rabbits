using Cysharp.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using SanderSaveli.Snake;
using System;
using UnityEngine;
using Zenject;

public class SnakeHead : TickableCellEntity
{
    public Vector2Int HeadPosition => new Vector2Int(CurrentCell.X, CurrentCell.Y);
    public Direction Direction { get; set; }
    public Action<Cell> OnCellChange { get; set; }

    [SerializeField] private SnakeMoveHandler _moveHandler;
    [SerializeField] private TailManager _tailManager;
    private SignalBus _signalBus;
    private bool _isNeedSpawnTail;
    private float _coyoteTime;

    [Inject]
    public void Construct(SignalBus signalBus, GameplayConfig gameplayConfig, LevelConfig levelConfig)
    {
        _signalBus = signalBus;
        _coyoteTime = gameplayConfig.CoyoteTime;
        Direction = levelConfig.start_direction;
    }

    public override void CollideWithHead(SnakeHead snake, out bool IsKillSnake)
    {
        IsKillSnake = true;
        snake.Die();
    }

    public override Type GetEntityType() => typeof(SnakeHead);

    public void Die()
    {
        _signalBus.Fire(new SignalGameEnd(GameEndStatus.Lose_collide));
    }

    public void AddLength()
    {
        _isNeedSpawnTail = true;
    }

    public void SetStartTailParts()
    {
        _tailManager.InitSnakeTail(this);
    }

    public async override void Tick()
    {
        _tailManager.MoveTailParts();

        if (_isNeedSpawnTail)
        {
            _tailManager.AddTailPart();
            _isNeedSpawnTail = false;
        }

        if(Direction != _moveHandler.GetActualDirection())
        {
            Direction = _moveHandler.GetActualDirection();
        }
        else
        {
            Cell currCell = CurrentCell;
            Vector2Int nextCellPos = HeadPosition + DirectionTool.DirectionToVector2(Direction);
            if (GameField.IsInBounds(nextCellPos))
            {
                Cell nextCell = GameField[nextCellPos.x, nextCellPos.y];
                CurrentCell = nextCell;
                OnCellChange?.Invoke(nextCell);
            }
            Direction = await WaitCoyoteTime();
            CurrentCell = currCell;
            OnCellChange?.Invoke(CurrentCell);
        }
        ChangeCell();
    }

    private void ChangeCell()
    {
        Vector2Int nextCellPos = HeadPosition + DirectionTool.DirectionToVector2(Direction);
        if (!GameField.IsInBounds(nextCellPos))
        {
            Die();
            return;
        }

        Cell nextCell = GameField[nextCellPos.x, nextCellPos.y];

        if (nextCell.IsOccupied)
        {
            nextCell.Entity.CollideWithHead(this, out bool isKill);
            if (isKill)
            {
                return;
            }
        }
        nextCell.SetEntity(this);
        CurrentCell = nextCell;
        OnCellChange?.Invoke(nextCell);
    }

    private async UniTask<Direction> WaitCoyoteTime()
    {
        float coyoteTimeCurr = _coyoteTime;
        while (Direction == _moveHandler.GetActualDirection())
        {
            coyoteTimeCurr -= Time.deltaTime;
            if( coyoteTimeCurr < 0 )
            {
                break;
            }
            await UniTask.Yield();
        }
        return _moveHandler.GetActualDirection();
    }
}
