using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public interface IGameField
    {
        public int FieldWidth { get; }
        public int FieldHeight { get; }
        public Matrix<Cell> Field { get; }

        public Cell this[int x, int y] { get; set; }

        public Vector3 CellToWorld(Vector2Int pos);

        public Vector3 CellToWorld(int x, int y);

        public bool IsInBounds(Vector2Int pos);

        public bool IsInBounds(int x, int y);

        public List<Cell> GetFreeCell();
    }
}
