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
            return path != null && path.Count > 0 ;
        }

        public List<T> GetPath(Vector2Int from, Vector2Int to)
        {
            ClearMatrix();
            PathNode a = _pathMatrix.GetValue(from);
            PathNode b = _pathMatrix.GetValue(to);

            List<PathNode> toSearch = new List<PathNode>() { a };
            HashSet<PathNode> processed = new HashSet<PathNode>();

            while (toSearch.Count > 0)
            {
                PathNode current = GetNearest(toSearch);
                toSearch.Remove(current);
                processed.Add(current);

                List<PathNode> neibhours = _neighbourLocator.GetNeighbors(current, PathMatrix);

                foreach (PathNode neibhour in neibhours)
                {
                    if (!neibhour.Passable)
                    {
                        continue;
                    }

                    bool isProcessed = processed.Contains(neibhour);
                    float costToNeighbors = _neighbourLocator.DistanceBetween(a, neibhour);

                    if (!isProcessed || costToNeighbors < neibhour.G)
                    {
                        neibhour.SetFrom(current);
                        neibhour.SetG(costToNeighbors);

                        if (!isProcessed)
                        {
                            neibhour.SetH(_neighbourLocator.DistanceBetween(neibhour, b));
                            toSearch.Add(neibhour);
                        }
                    }
                }
            }

            if (b.ComeFrom == null)
            {
                return null;
            }

            return GetPath(a, b);
        }

        private Matrix<PathNode> IniPathMatrix(Matrix<T> originalMatrix)
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

        private void ClearMatrix()
        {
            List<PathNode> allNodes = _pathMatrix.AllValues();

            foreach (PathNode node in allNodes)
            {
                node.SetG(0);
                node.SetH(0);
                node.SetFrom(null);
            }
        }

        private T GetOrigin(PathNode node) =>
            _originalMatrix.GetValue(node.Position);

        private List<T> GetPath(PathNode a, PathNode b)
        {
            List<T> path = new List<T>
            {
                GetOrigin(b)
            };

            PathNode curr = b.ComeFrom;
            while (curr != a)
            {
                T node = GetOrigin(curr);
                if (path.Contains(node))
                {
                    throw new Exception($"Loop expected in path! {curr.Position}");
                }

                path.Add(GetOrigin(curr));
                curr = curr.ComeFrom;
            }
            path.Reverse();
            return path;
        }
    }
}
