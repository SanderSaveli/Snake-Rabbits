using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ObstacleSpawner : CellEntitySpawner<Obstacle>
    {
        [Header("Params")]
        [SerializeField] private int _obstacleCount;

        private IGameField _gameField;

        [Inject]
        public void Construct(IGameField gameField)
        {
            _gameField = gameField;
        }

        public void SpawnObstracles()
        {
            List<Cell> freeCells = _gameField.GetFreeCell();

            for (int i = 0; i < _obstacleCount; i++)
            {
                if (freeCells.Count == 0)
                {
                    Debug.LogError("There is not enough free cells for spawn obstacle!");
                    break;
                }
                Cell randomCell = freeCells[Random.Range(0, freeCells.Count)];
                Spawn(randomCell);
                freeCells.Remove(randomCell);
            }
        }
    }
}
