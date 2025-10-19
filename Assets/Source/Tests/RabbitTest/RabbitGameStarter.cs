using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class RabbitGameStarter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameLoop _gameLoop;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private CarrotSpawner _carrotSpawner;
        [SerializeField] private RabbitSpawner _rabbitSpawner;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            Time.timeScale = 1;
            _obstacleSpawner.SpawnObstracles();
            _carrotSpawner.SpawnCarrot();
            _rabbitSpawner.SpawnRabbits();
            _gameLoop.StartGameLoop();
        }

        private void OnEnable()
        {
            _gameField.EnsureField();
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void HandleGameEnd(SignalGameEnd ctx)
        {
            _gameLoop.EndGameLoop();
        }
    }
}
