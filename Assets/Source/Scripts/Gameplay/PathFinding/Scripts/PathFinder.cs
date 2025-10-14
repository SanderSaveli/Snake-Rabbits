using System.Collections.Generic;

namespace SanderSaveli.Pathfinding
{
    public class PathFinder
    {
        private Matrix<PathNode> _matrix;
        private FourSideNeighborLockator _neighbourLocator;

        public PathFinder(Matrix<PathNode> matrix)
        {
            _matrix = matrix;
            _neighbourLocator = new FourSideNeighborLockator(_matrix);
        }

        public void SetMatrix(Matrix<PathNode> matrix)
        {
            _matrix = matrix;
        }

        public List<PathNode> GetPath(PathNode a, PathNode b)
        {
            ClearMatrix();
            List<PathNode> path = new List<PathNode>();

            List<PathNode> toSearch = new List<PathNode>() { a };
            HashSet<PathNode> processed = new HashSet<PathNode>();

            while (toSearch.Count > 0)
            {
                PathNode current = GetNearest(toSearch);
                toSearch.Remove(current);
                processed.Add(current);

                List<PathNode> neibhours = _neighbourLocator.GetNeighbors(current);

                foreach (PathNode neibhour in neibhours)
                {
                    if (!neibhour.Passable)
                    {
                        continue;
                    }

                    bool isProcessed = processed.Contains(neibhour);
                    float coastToNeighbors = _neighbourLocator.DistanceBetween(current, neibhour);

                    if (!isProcessed || coastToNeighbors < neibhour.G)
                    {
                        neibhour.SetFrom(current);
                        neibhour.SetG(coastToNeighbors);

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
            path.Add(b);
            PathNode curr = b.ComeFrom;
            while (curr != a)
            {
                if (path.Contains(curr))
                {
                    throw new System.Exception($"Loop expected in path! {curr.Position}");
                }

                path.Add(curr);
                curr = curr.ComeFrom;
            }
            path.Reverse();
            return path;
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
            List<PathNode> allNodes = _matrix.AllValues();

            foreach (PathNode node in allNodes)
            {
                node.SetG(0);
                node.SetH(0);
                node.SetFrom(null);
            }
        }
    }
}
