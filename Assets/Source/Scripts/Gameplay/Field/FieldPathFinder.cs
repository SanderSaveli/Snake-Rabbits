using SanderSaveli.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class FieldPathFinder : MonoBehaviour, IFieldPathFinder
    {
        private IGameField _gameField;
        private PathFinder<Cell> _pathFinder;

        [Inject]
        public void Construct(IGameField gameField)
        {
            _gameField = gameField;
        }

        public void Start()
        {
            INeighborLocator locator = new FourSideNeighborLockator();
            _pathFinder = new PathFinder<Cell>(_gameField.Field, IsPassable, locator);
        }

        public List<Cell> FindAll<T>() where T : CellEntity
        {
            List<Cell> result = new List<Cell>();

            List<Cell> allCells = _gameField.Field.AllValues();

            foreach (var cell in allCells)
            {
                if (cell.IsOccupied)
                {
                    if (cell.Entity.GetType() == typeof(T))
                    {
                        result.Add(cell);
                    }
                }
            }
            return result;
        }

        public Cell GetNearestCellWithEntity<T>(Cell from, out List<Cell> path) where T : CellEntity
        {
            List<Cell> allEntities = FindAll<T>();
            Cell nearestCell = null;
            int minPath = int.MaxValue;
            path = null;
            _pathFinder.IniPathMatrix(_gameField.Field);
            foreach (Cell cell in allEntities)
            {
                if (_pathFinder.TryGetPath(from.Position, cell.Position, out List<Cell> pathToCell))
                {
                    if (pathToCell.Count < minPath)
                    {
                        nearestCell = cell;
                        minPath = pathToCell.Count;
                        path = pathToCell;
                    }
                }
            }

            return nearestCell;
        }

        public bool TryFindPath(Cell from, Cell to, out List<Cell> path)
        {
            _pathFinder.IniPathMatrix(_gameField.Field);
            return _pathFinder.TryGetPath(from.Position, to.Position, out path);
        }

        private bool IsPassable(Cell cell)
        {
            return !cell.IsOccupied;
        }
    }
}
