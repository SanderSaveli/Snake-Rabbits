using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class CellEntitySpawner<T> : MonoBehaviour where T : CellEntity
    {
        [Header("Components")]
        [SerializeField] private Transform _parent;

        [Header("Prefabs")]
        [SerializeField] private T _objectPrefab;

        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        public T Spawn(Cell cell)
        {
            if(cell.IsOccupied)
            {
                throw new System.ArgumentException($"There is anoter Entity on cell: {cell.Position}. \n Cell entity: {cell.Entity.GetEntityType()}");
            }

            T entity = _container.InstantiatePrefabForComponent<T>(_objectPrefab, _parent);
            entity.SetStartCell(cell);
            return entity;
        }

    }
}
