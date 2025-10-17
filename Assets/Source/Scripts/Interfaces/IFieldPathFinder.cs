using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public interface IFieldPathFinder
    {
        public List<Cell> FindAll<T>() where T : CellEntity;

        public Cell GetNearestCellWithEntity<T>(Cell from, out List<Cell> path) where T : CellEntity;

        public bool TryFindPath(Cell from, Cell to, out List<Cell> path);
    }
}
