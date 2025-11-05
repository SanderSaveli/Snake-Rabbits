using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class RandomSpawner<T> : CellEntitySpawner<T> where T : CellEntity
    {
        [Header("Params")]
        [SerializeField] private int _count;

        private IGameField _gameField;

        [Inject]
        public void Construct(IGameField gameField)
        {
            _gameField = gameField;
        }

        public override void SpawnAll()
        {
            SpawnRandom(_count);
        }

        public void SpawnRandom(int count = 1)
        {
            List<Cell> freeCells = _gameField.GetFreeCell();

            for (int i = 0; i < count; i++)
            {
                if (freeCells.Count == 0)
                {
                    Debug.LogError($"There is not enough free cells for spawn entity {_objectPrefab.EntityType}!");
                    return;
                }
                Cell randomCell = freeCells[Random.Range(0, freeCells.Count)];
                Spawn(randomCell);
                freeCells.Remove(randomCell);
            }
        }
    }
}
