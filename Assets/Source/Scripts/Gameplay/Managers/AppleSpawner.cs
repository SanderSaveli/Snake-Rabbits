using SanderSaveli.UDK;
using SanderSaveli.UDK.Tools;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class AppleSpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform _appleParent;

        [Header("Prefabs")]
        [SerializeField] private Apple _apple;

        private ObjectPool<Apple> _applePool;
        private GameField _gameField;
        private SignalBus _signalBus;
        private DiContainer _container;

        [Inject]
        public void Construct(SignalBus signalBus, GameField gameField, DiContainer diContainer)
        {
            _gameField = gameField;
            _signalBus = signalBus;
            _container = diContainer;
        }

        private void Awake()
        {
            _applePool = new InjectionObjectPool<Apple>(_container, _apple, _appleParent);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalAppleEated>(SpawnApple);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalAppleEated>(SpawnApple);
        }

        public void SpawnApple()
        {
            Apple apple = _applePool.Get();
            List<Cell> freeCells = _gameField.GetFreeCell();
            apple.SetStartCell(freeCells[Random.Range(0, freeCells.Count)]);
        }
    }
}
