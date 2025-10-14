using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public interface INeighborLocator
    {
        public float DistanceBetween(Vector2Int a, Vector2Int b);

        public List<Vector2Int> GetNeighbors(Vector2Int position);
    }
}
