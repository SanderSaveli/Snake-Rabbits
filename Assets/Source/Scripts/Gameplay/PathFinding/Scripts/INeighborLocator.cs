using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public interface INeighborLocator
    {
        public float DistanceBetween(PathNode a, PathNode b);
        public List<PathNode> GetNeighbors(PathNode position, Matrix<PathNode> matrix);
        public List<Vector2Int> GetNeighborsCoordinate(Vector2Int position);
    }
}
