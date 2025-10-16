using SanderSaveli.Snake;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class PathFinderTester : MonoBehaviour
    {
        [SerializeField] private FieldGenerator _gameField;
        [SerializeField] private CellPathSelector _cellSelector;
        [Header("Properties")]
        [SerializeField] private int FieldWidth;
        [SerializeField] private int FieldHeight;

        private Matrix<CellView> _cellViews;
        private PathFinder<CellView> _finder;
        private List<PathCellView> _pathCells;

        private void Start()
        {
            _pathCells = new List<PathCellView>();
            _cellViews = _gameField.GenerateField(FieldWidth, FieldHeight);
            _finder = new PathFinder<CellView>(_cellViews, IsNotObstacle, new FourSideNeighborLockator());
        }

        private void Update()
        {
            if (_cellSelector.StartCell != null
                && _cellSelector.HoverCell != null
                && _cellSelector.StartCell != _cellSelector.HoverCell)
            {
                Vector2Int startCellPos = GetCoordinate(_cellSelector.StartCell);
                Vector2Int endCellPos = GetCoordinate(_cellSelector.HoverCell);
                List<CellView> path = _finder.GetPath(startCellPos, endCellPos);
                ClearPathCells();
                if (path == null)
                {
                    return;
                }
                Transform previousCell = null;

                foreach (CellView pathNode in path)
                {
                    PathCellView pathCellView = pathNode as PathCellView;
                    pathCellView.SelectAsPath(true, previousCell);
                    previousCell = pathCellView.transform;
                    _pathCells.Add(pathCellView);
                }
                _cellSelector.StartCell.SelectAsStart();
            }
        }

        private void ClearPathCells()
        {
            foreach (PathCellView pathNode in _pathCells)
            {
                pathNode.SelectAsPath(false);
            }
            _pathCells.Clear();
        }

        private bool IsNotObstacle(CellView matrix)
        {
            float r = Random.Range(0f, 1f);
            bool isObstacle = r < 0.15f;
            if(isObstacle)
            {
                (matrix as PathCellView).SelectAsObstracle();
            }
            return !isObstacle;
        }

        private Vector2Int GetCoordinate(CellView cellView)
        {
            for (int x = 0; x < _cellViews.Width; x++)
            {
                for (int y = 0; y < _cellViews.Height; y++)
                {
                    if (_cellViews[x, y] == cellView)
                        return new Vector2Int(x, y);
                }
            }
            return new Vector2Int(0, 0);
        }
    }
}
