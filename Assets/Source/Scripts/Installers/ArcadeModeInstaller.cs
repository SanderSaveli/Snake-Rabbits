using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ArcadeModeInstaller : GameModeInstaller
    {
        [SerializeField] private GraficConfig _graicConfig;
        [SerializeField] private GameplayConfig _gameplayConfig;
        [SerializeField] private LevelConfig _levelConfig;

        [SerializeField] private ScoreManager _scoreManager;


        public override void InstallBindings()
        {
            _container.Bind<GameplayConfig>().FromInstance(_gameplayConfig).AsSingle().NonLazy();
            _container.Bind<GraficConfig>().FromInstance(_graicConfig).AsSingle().NonLazy();
            _container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle().NonLazy();

            _container.Bind<IScoreManager>().FromInstance(_scoreManager).AsSingle().NonLazy();
        }
    }
}
