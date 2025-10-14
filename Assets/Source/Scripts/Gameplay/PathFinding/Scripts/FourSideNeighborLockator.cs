using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class FourSideNeighborLockator
    {
        private Matrix<PathNode> _matrix;

        public FourSideNeighborLockator(Matrix<PathNode> matrix)
        {
            _matrix = matrix;
        }

        public float DistanceBetween(PathNode a, PathNode b)
        {
            int deltaX = Mathf.Abs(a.Position.x - b.Position.x);
            int deltaY = Mathf.Abs(a.Position.y - b.Position.y);

            return deltaX + deltaY;
        }

        public List<PathNode> GetNeighbors(PathNode position)
        {
            List<Vector2Int> neighbors = GetNeighborsCoordinate(position.Position);
            List<PathNode> pathNodes = new List<PathNode>();

            foreach (Vector2Int node in neighbors)
            {
                if(node.x >=0  && node.y >=0 && node.x < _matrix.Width && node.y < _matrix.Height)
                {
                    pathNodes.Add(_matrix[node.x, node.y]);
                }
            }

            return pathNodes;
        }

        public List<Vector2Int> GetNeighborsCoordinate(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
                {
                    position + Vector2Int.up,
                    position + Vector2Int.right,
                    position + Vector2Int.down,
                    position + Vector2Int.left,
                };
            return neighbors;
        }
    }
}
