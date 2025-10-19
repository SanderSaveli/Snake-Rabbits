using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameStarter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameLoop _gameLoop;
        [SerializeField] private CellEntitySpawner<Obstacle> _obstacleSpawner;
        [SerializeField] private CellEntitySpawner<Carrot> _carrotSpawner;
        [SerializeField] private CellEntitySpawner<Rabbit> _rabbitSpawner;

        [Header("Prefabs")]
        [SerializeField] private SnakeHead _snakeHead;
        [SerializeField] private SnakeTail _snakeSegment;

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
            _obstacleSpawner.SpawnAll();
            _carrotSpawner.SpawnAll();
            _rabbitSpawner.SpawnAll();
            _gameLoop.StartGameLoop();
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
