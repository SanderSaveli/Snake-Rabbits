using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public interface IFieldPathFinder
    {
        public Cell GetNearestCellWithEntity<T>(Cell from, out List<Cell> path) where T : CellEntity;

        public bool TryFindPath(Cell from, Cell to, out List<Cell> path);
    }
}
