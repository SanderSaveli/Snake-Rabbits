using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class PathFinder<T>
    {
        public Matrix<PathNode> PathMatrix => _pathMatrix;

        private Matrix<PathNode> _pathMatrix;
        private Matrix<T> _originalMatrix;
        private INeighborLocator _neighbourLocator;
        private Func<T, bool> _isPassableDelegate;
        private int _maxIteration = 100000;

        public PathFinder(Matrix<T> matrix, Func<T, bool> isPassedDelegate, INeighborLocator neighborLocator)
        {
            _originalMatrix = matrix;
            _isPassableDelegate = isPassedDelegate;
            _pathMatrix = IniPathMatrix(matrix);
            _neighbourLocator = neighborLocator;
        }

        public bool TryGetPath(Vector2Int from, Vector2Int to, out List<T> path)
        {
            path = GetPath(from, to);
            return path != null && path.Count > 0;
        }

        public List<T> GetPath(Vector2Int from, Vector2Int to)
        {
            _pathMatrix = IniPathMatrix(_originalMatrix);
            int i = 0;

            PathNode a = _pathMatrix.GetValue(from);
            PathNode b = _pathMatrix.GetValue(to);

            // ✅ исправлено: инициализация G и H для старта
            // Раньше G=0 сбивало логику оценки пути (все соседи переоценивались).
            a.SetG(0);
            a.SetH(_neighbourLocator.DistanceBetween(a, b));

            List<PathNode> toSearch = new List<PathNode>() { a };

            HashSet<PathNode> processed = new HashSet<PathNode>();

            while (true)
            {
                i++;
                if (i > _maxIteration)
                    throw new Exception("max iteration limit!");

                if (toSearch.Count == 0)
                    break;
                PathNode current = GetNearest(toSearch);
                toSearch.Remove(current); processed.Add(current);

                // ✅ исправлено: сравнение по координате, а не по ссылке
                // Раньше current == b почти никогда не срабатывал
                if (current.Position == b.Position)
                {
                    return GetPath(a, current);
                }

                List<PathNode> neighbours = _neighbourLocator.GetNeighbors(current, PathMatrix);

                foreach (PathNode neighbour in neighbours)
                {
                    // ✅ исправлено: разрешаем непроходимую конечную точку, остальные — пропускаем
                    if (!neighbour.Passable && neighbour.Position != b.Position)
                    {
                        continue;
                    }

                    bool isProcessed = processed.Contains(neighbour);
                    float costToNeighbors = current.G + _neighbourLocator.DistanceBetween(current, neighbour);

                    // ✅ исправлено: G теперь инициализируется float.MaxValue, так что сравнение корректное
                    if (!isProcessed || costToNeighbors < neighbour.G)
                    {
                        neighbour.SetFrom(current);
                        neighbour.SetG(costToNeighbors);

                        // ✅ исправлено: усилили эвристику, чтобы алгоритм "тянулся" к цели быстрее
                        neighbour.SetH(_neighbourLocator.DistanceBetween(neighbour, b) * 1.1f);

                        if (!isProcessed && !toSearch.Contains(neighbour))
                            toSearch.Add(neighbour);
                    }
                }
            }

            Debug.LogWarning($"Path not found after {i} iterations");
            return null;
        }

        public Matrix<PathNode> IniPathMatrix(Matrix<T> originalMatrix)
        {
            Matrix<PathNode> pathMatrix = new Matrix<PathNode>(originalMatrix.Width, originalMatrix.Height);
            for (int x = 0; x < originalMatrix.Width; x++)
            {
                for (int y = 0; y < originalMatrix.Height; y++)
                {
                    bool isPassable = _isPassableDelegate(originalMatrix[x, y]);
                    PathNode node = new PathNode(new Vector2Int(x, y), isPassable);
                    pathMatrix[x, y] = node;
                }
            }

            return pathMatrix;
        }

        private void LogMatrix()
        {
            string debug = "";
            for (int y = _pathMatrix.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < _pathMatrix.Width; x++)
                {
                    bool isPassable = _pathMatrix[x, y].Passable;
                    if (isPassable)
                    {
                        debug += "_";
                    }
                    else
                    {
                        debug += "*";
                    }
                }
                debug += "\n";
            }
            Debug.Log(debug);
        }

        private PathNode GetNearest(List<PathNode> variants)
        {
            float minDistance = float.MaxValue;
            PathNode nearest = variants[0];
            foreach (PathNode variant in variants)
            {
                if (variant.F < minDistance)
                {
                    minDistance = variant.F;
                    nearest = variant;
                }
            }
            return nearest;
        }

        private T GetOrigin(PathNode node) =>
            _originalMatrix.GetValue(node.Position);

        private List<T> GetPath(PathNode a, PathNode b)
        {
            List<T> path = new List<T> { GetOrigin(b) };
            PathNode curr = b.ComeFrom;

            while (curr != a)
            {
                if (curr == null)
                    throw new Exception("Path reconstruction failed — missing ComeFrom");

                T node = GetOrigin(curr);
                if (path.Contains(node))
                    throw new Exception($"Loop expected in path! {curr.Position}");

                path.Add(node);
                curr = curr.ComeFrom;
            }

            path.Reverse();
            return path;
        }
    }
}
