using SanderSaveli.Snake;
using System.Collections;
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
        
        private Matrix<PathNode> _pathNodes;
        private Matrix<CellView> _cellViews;
        private PathFinder _finder;
        private List<PathCellView> _pathCells;

        private void Start()
        {
            _pathCells = new List<PathCellView>();
            _cellViews = _gameField.GenerateField(FieldWidth, FieldHeight);
            _pathNodes = IniMatrix(_cellViews);
            _finder = new PathFinder(_pathNodes);
        }

        private void Update()
        {
            if(_cellSelector.StartCell !=  null 
                && _cellSelector.HoverCell != null 
                && _cellSelector.StartCell != _cellSelector.HoverCell)
            {
                Vector2Int startCellPos = GetCoordinate(_cellSelector.StartCell);
                Vector2Int endCellPos = GetCoordinate(_cellSelector.HoverCell);
                List<PathNode> pathNodes = _finder.GetPath(
                    _pathNodes.GetValue(startCellPos), _pathNodes.GetValue(endCellPos));
                ClearPathCells();
                if(pathNodes == null)
                {
                    return;
                }
                Transform previousCell = null;
                foreach (PathNode pathNode in pathNodes)
                {
                    CellView cellView = _cellViews.GetValue(pathNode.Position);
                    PathCellView pathCellView = cellView as PathCellView;
                    pathCellView.SelectAsPath(true, previousCell);
                    previousCell = pathCellView.transform;
                    _pathCells.Add(pathCellView);
                }
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

        private Matrix<PathNode> IniMatrix(Matrix<CellView> matrix)
        {
            Matrix<PathNode> pathNodes = new Matrix<PathNode>(matrix.Width, matrix.Height);

            for(int x = 0; x < matrix.Width; x++)
            {
                for(int y = 0; y < matrix.Height; y++)
                {
                    float r = Random.Range(0f, 1f);
                    bool isObstacle = r < 0.15f;
                    PathNode node = new PathNode(new Vector2Int(x, y), !isObstacle);
                    pathNodes[x, y] = node;

                    if (isObstacle)
                    {
                        PathCellView pathCellView = matrix[x, y] as PathCellView;
                        pathCellView.SelectAsObstracle();
                    }
                }
            }
            return pathNodes;
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
