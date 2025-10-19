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
            Debug.Log("____Tick___");
            if (_action != null)
            {
                Debug.Log("Clockdown");
                ClockdownAction();
            }
            else
            {
                Debug.Log("DetermineAction");
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
                Debug.Log("There is no Path!");
                return;
            }
            foreach(var cell in path)
            {
                Debug.Log(cell.Position);
            }
            Cell nextCell = path[0];

            if (IsLookAtCell(nextCell))
            {
                Debug.Log("I Look at cell!");
                if (path.Count == 1)
                {
                    Debug.Log("I Look at Carrot, start eat!");
                    _fsm.ChangeState<EatCarrotState>();
                }
                else
                {
                    MoveForward();
                }
            }
            else
            {
                Debug.Log("INeed rotate");
                Direction direction = GetCellOrentation(nextCell);
                if(DirectionTool.IsOpposite(_rabbit.LookAt, direction))
                {
                    RotateTo(GetRandomTurnToward());
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
            Debug.Log("Start rotate");
            _currentActionTimer = _rotationDuration;
            _action = () => _rabbit.Rotate(direction);
        }

        private void MoveForward()
        {
            Debug.Log("Start move forvard");
            _currentActionTimer = _moveDuration;
            _action = () => _rabbit.MoveForward();
        }

        private Direction GetRandomTurnToward()
        {
            bool turnLeft = UnityEngine.Random.Range(0, 2) == 1;
            return turnLeft ?  DirectionTool.TurnLeft(_rabbit.LookAt) : DirectionTool.TurnRight(_rabbit.LookAt);
        }
    }
}
