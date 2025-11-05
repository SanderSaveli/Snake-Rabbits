using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class ConfigSpawner<T> : CellEntitySpawner<T> where T : CellEntity
    {
        protected LevelConfig _levelConfig;
        private IGameField _gameField;

        [Inject]
        public void Construct(LevelConfig levelConfig, IGameField gameField)
        {
            _levelConfig = levelConfig;
            _gameField = gameField;
        }

        public override void SpawnAll()
        {
            List<Vector2Int> cellPositions = GetAllEntityCoordinate(_levelConfig);

            foreach (var cellPosition in cellPositions)
            {
                Cell cell = _gameField[cellPosition.x, cellPosition.y];
                Spawn(cell);
            }
        }

        protected abstract List<Vector2Int> GetAllEntityCoordinate(LevelConfig levelConfig);
    }
}
