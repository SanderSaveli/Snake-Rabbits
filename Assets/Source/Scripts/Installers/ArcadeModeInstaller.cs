using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ArcadeModeInstaller : GameModeInstaller
    {
        [SerializeField] private ConfigProvider _configProvider;

        [SerializeField] private ArcadeHealthManager _arcadeHealthManager;
        [SerializeField] private ArcadeScoreManager _scoreManager;

        public override void InstallBindings()
        {
            _container.Inject(_configProvider);
            _container.Bind<GameplayConfig>().FromInstance(_configProvider.GameplayConfig).AsSingle().NonLazy();
            _container.Bind<GraficConfig>().FromInstance(_configProvider.GraficConfig).AsSingle().NonLazy();
            _container.Bind<LevelConfig>().FromInstance(_configProvider.LevelConfig).AsSingle().NonLazy();

            _container.Bind<IScoreManager>().FromInstance(_scoreManager).AsSingle().NonLazy();
            _container.Bind<IHealthManager>().FromInstance(_arcadeHealthManager).AsSingle().NonLazy();
        }
    }
}
