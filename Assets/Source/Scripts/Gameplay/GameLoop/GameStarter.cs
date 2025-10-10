using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameLoop _gameLoop;
        [Header("Prefabs")]
        [SerializeField] private SnakeHead _snakeHead;
        [SerializeField] private SnakeTail _snakeSegment;
        [SerializeField] private AppleSpawner _appleSpawner;
        private SignalBus _signalBus;
        private DiContainer _container;
        private LevelConfig _levelConfig;

        [Inject]
        public void Construct(SignalBus signalBus, DiContainer container, LevelConfig levelConfig)
        {
            _signalBus = signalBus;
            _container = container;
            _levelConfig = levelConfig;
        }

        private void Start()
        {
            Time.timeScale = 1;
            _gameField.EnsureField();
            InitSnake();
            _gameLoop.StartGameLoop();
            _appleSpawner.SpawnApple();
        }

        private void OnEnable()
        {
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

        private void InitSnake()
        {
            SnakeHead snake = _container.InstantiatePrefabForComponent<SnakeHead>(_snakeHead);
            Cell snakeStartCell = _gameField[_gameField.FieldWidth / 2, _gameField.FieldHeight / 2];
            snake.SetStartCell(snakeStartCell);
            snake.Direction = _levelConfig.startDirection;
            snake.SetStartTailParts();
        }
    }
}
