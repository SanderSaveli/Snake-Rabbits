using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class RabbitGameStarter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameLoop _gameLoop;
        [SerializeField] private CellEntitySpawner<Obstacle> _obstacleSpawner;
        [SerializeField] private CellEntitySpawner<Carrot> _carrotSpawner;
        [SerializeField] private CellEntitySpawner<Rabbit> _rabbitSpawner;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            Time.timeScale = 1;
            _obstacleSpawner.SpawnAll();
            _carrotSpawner.SpawnAll();
            _rabbitSpawner.SpawnAll();
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
