using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class CellEntitySpawner<T> : MonoBehaviour where T : CellEntity
    {
        [Header("Components")]
        [SerializeField] protected Transform _parent;

        [Header("Prefabs")]
        [SerializeField] protected T _objectPrefab;

        protected DiContainer _container;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        public virtual T Spawn(Cell cell)
        {
            if(cell.IsOccupied)
            {
                throw new System.ArgumentException($"There is anoter Entity on cell: {cell.Position}. \n Cell entity: {cell.Entity.GetEntityType()}");
            }

            T entity = _container.InstantiatePrefabForComponent<T>(_objectPrefab, _parent);
            entity.SetStartCell(cell);
            return entity;
        }

        public abstract void SpawnAll();
    }
}
