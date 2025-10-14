using UnityEngine;

namespace SanderSaveli.Pathfinding
{
    public class PathNode
    {
        public PathNode(Vector2Int position, bool passable)
        {
            Position = position;
            Passable = passable;
        }

        public PathNode ComeFrom { get; private set; }
        public Vector2Int Position { get; private set; }
        public bool Passable { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public void SetFrom(PathNode node) =>
            ComeFrom = node;

        public void SetG(float g) => G = g;

        public void SetH(float h) => H = h;
    }
}
