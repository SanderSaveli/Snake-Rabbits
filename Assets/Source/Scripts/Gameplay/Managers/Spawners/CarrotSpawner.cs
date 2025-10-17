using SanderSaveli.UDK.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class CarrotSpawner : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private int _carrotCount;

        [Header("Components")]
        [SerializeField] private Transform _carrotParent;

        [Header("Prefabs")]
        [SerializeField] private Carrot _carrot;

        private IGameField _gameField;
        private DiContainer _container;

        private List<Carrot> _carrotList;

        [Inject]
        public void Construct(IGameField gameField, DiContainer diContainer)
        {
            _gameField = gameField;
            _container = diContainer;
        }

        public void SpawnCarrot()
        {
            List<Cell> freeCells = _gameField.GetFreeCell();
            _carrotList = new List<Carrot>();

            for (int i = 0; i < _carrotCount; i++)
            {
                if(freeCells.Count <= 0)
                {
                    Debug.LogError("There is not enought free cells for carrot");
                    break;
                }

                Carrot carrot = _container.InstantiatePrefabForComponent<Carrot>(_carrot, _carrotParent);
                Cell randomCell = freeCells[Random.Range(0, freeCells.Count)];
                freeCells.Remove(randomCell);
                carrot.SetStartCell(randomCell);
                _carrotList.Add(carrot);
            }
        }
    }
}
