using SanderSaveli.UDK;
using SanderSaveli.UDK.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ObstracleSpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform _obstacleParent;

        [Header("Prefabs")]
        [SerializeField] private Obstacle _obstacle;

        [Header("Params")]
        [SerializeField] private int _obstracleCount;

        private ObjectPool<Obstacle> _obstaclePool;
        private GameField _gameField;
        private DiContainer _container;

        [Inject]
        public void Construct(GameField gameField, DiContainer diContainer)
        {
            _gameField = gameField;
            _container = diContainer;
        }

        private void Awake()
        {
            _obstaclePool = new InjectionObjectPool<Obstacle>(_container, _obstacle, _obstacleParent);
        }

        public void SpawnObstracles()
        {
            List<Cell> freeCells = _gameField.GetFreeCell();

            for (int i = 0; i < _obstracleCount; i++)
            {
                if(freeCells.Count == 0)
                {
                    Debug.LogError("There is not enough free cells for spawn obstacle!");
                    break;
                }
                Cell randomCell = freeCells[Random.Range(0, freeCells.Count)];
                Obstacle obstacle = _obstaclePool.Get();
                freeCells.Remove(randomCell);
                obstacle.SetStartCell(randomCell);
            }
        }
    }
}
