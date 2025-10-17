using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class MoveToCarrotState : IRabbitState
    {
        private Rabbit _rabbit;
        private FSM<IRabbitState> _fsm;
        private IFieldPathFinder _fieldPathFinder;
        private GameplayConfig _gameplayConfig;

        private int _moveDuration;
        private int _rotationDuration;
        private int _currentActionTimer;
        private Action _action;

        public MoveToCarrotState(IFieldPathFinder fieldPathFinder, RabbitConfig gameplayConfig)
        {
            _fieldPathFinder = fieldPathFinder;
            _moveDuration = gameplayConfig.MoveDuration;
            _rotationDuration = gameplayConfig.RotateDuration;
        }

        public void Initialize(Rabbit rabbit, FSM<IRabbitState> fsm)
        {
            _rabbit = rabbit;
            _fsm = fsm;
        }

        public void OnEnter()
        {

        }

        public void OnUpdate()
        {
            if(_action != null)
            {
                ClockdownAction();
            }
            else
            {
                DetermineNextAction();
            }
        }

        public void OnExit()
        {

        }

        private void ClockdownAction()
        {
            _currentActionTimer--;
            if (_currentActionTimer <= 0)
            {
                _action.Invoke();
                _action = null;
            }
        }

        private void DetermineNextAction()
        {
            Cell targetCell = 
                _fieldPathFinder.GetNearestCellWithEntity<Carrot>(_rabbit.CurrentCell, out List<Cell> path);

            if (targetCell == null)
            {
                return;
            }

            Cell nextCell= path[0];
            if(IsLookAtCell(nextCell))
            {
                MoveForward();
            }
            else
            {
                Direction direction = GetCellOrentation(nextCell);
                if(DirectionTool.IsOpposite(_rabbit.LookAt, direction))
                {
                    GetRandomTurnToward(direction);
                }
                else
                {
                    RotateTo(direction);
                }
            }
        }

        private bool IsLookAtCell(Cell cell) =>
            _rabbit.GetFowardCell() == cell;

        private Direction GetCellOrentation(Cell cell)
        {
            Vector2Int pos = _rabbit.CurrentCell.Position;
            Vector2Int targetPos = cell.Position;

            Vector2Int diff = targetPos - pos;

            if (diff == Vector2Int.up)
                return Direction.Up;
            if (diff == Vector2Int.down)
                return Direction.Down;
            if (diff == Vector2Int.left)
                return Direction.Left;
            if (diff == Vector2Int.right)
                return Direction.Right;

            throw new Exception($"Cells is not neibhours: {pos}, {targetPos}");
        }

        private void RotateTo(Direction direction)
        {
            _currentActionTimer = _rotationDuration;
            _action = () => _rabbit.Rotate(direction);
        }

        private void MoveForward()
        {
            _currentActionTimer = _moveDuration;
            _action = () => _rabbit.MoveForward();
        }

        private Direction GetRandomTurnToward(Direction target)
        {
            bool turnLeft = UnityEngine.Random.Range(0, 2) == 1;
            return turnLeft ?  DirectionTool.TurnLeft(_rabbit.LookAt) : DirectionTool.TurnRight(_rabbit.LookAt);
        }
    }
}
