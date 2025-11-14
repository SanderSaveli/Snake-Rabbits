using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class TailManager : MonoBehaviour
    {
        public List<SnakeTail> TailsParts;
        public int TailLength => TailsParts.Count;
        public Action OnChangeTailCount;

        [SerializeField] private SnakeTail _snakeTail;
        private SnakeHead _head;
        private LevelConfig _levelConfig;
        private DiContainer _container;
        private IGameField _gameField;
        private Cell _lastPassedCell;

        [Inject]
        public void Construct(DiContainer container, LevelConfig levelConfig, IGameField gameField)
        {
            _container = container;
            _levelConfig = levelConfig;
            _gameField = gameField;
        }

        public void InitSnakeTail(SnakeHead head)
        {
            _head = head;
            TailsParts = new List<SnakeTail>();
            Direction tailDirection = DirectionTool.GetOpposite(head.Direction);
            Cell previousCell = head.CurrentCell;
            Debug.Log(_levelConfig.start_segmets);
            for (int i = 0; i < _levelConfig.start_segmets; i++)
            {
                Cell curentCell = GetNextFreeCell(previousCell, tailDirection, out tailDirection);
                TailsParts.Add(SpawnTail(curentCell));
                previousCell = curentCell;
            }
        }

        public void MoveTailParts()
        {
            Cell PreviousCell = _head.CurrentCell;

            foreach (SnakeTail tail in TailsParts)
            {
                Cell tailCell = tail.CurrentCell;
                tail.ChangeCell(PreviousCell);
                PreviousCell = tailCell;
            }

            _lastPassedCell = PreviousCell;
            PreviousCell.SetEntity(null);
        }

        public void AddTailPart()
        {
            Cell curentCell = _lastPassedCell;
            TailsParts.Add(SpawnTail(curentCell));

            OnChangeTailCount?.Invoke();
        }

        private SnakeTail SpawnTail(Cell cell)
        {
            SnakeTail snakeTail = _container.InstantiatePrefabForComponent<SnakeTail>(_snakeTail);
            snakeTail.SetStartCell(cell);
            return snakeTail;
        }

        private Cell GetNextFreeCell(Cell from, Direction direction, out Direction afterDirection)
        {
            Vector2Int checkDirection = from.Position + DirectionTool.DirectionToVector2(direction);
            if (_gameField.IsInBounds(checkDirection))
            {
                Cell checkCell = _gameField[checkDirection.x, checkDirection.y];
                if (!checkCell.IsOccupied)
                {
                    afterDirection = direction;
                    return checkCell;
                }
            }

            throw new System.NotImplementedException("TO DO: CHECK FOR SIDE CELLS");
        }
    }
}
