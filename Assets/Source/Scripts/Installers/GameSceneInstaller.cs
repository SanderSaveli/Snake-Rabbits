using SanderSaveli.Snake;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private ConfigProvider _configProvider;
    [SerializeField] private GameField _gameField;
    [SerializeField] private GameLoop _gameLoop;
    [SerializeField] private ScoreManager _scoreManager;

    public override void InstallBindings()
    {
        Container.Bind<GameplayConfig>().FromInstance(_configProvider.GameplayConfig).AsSingle().NonLazy();
        Container.Bind<GraficConfig>().FromInstance(_configProvider.GraficConfig).AsSingle().NonLazy();
        Container.Bind<LevelConfig>().FromInstance(_configProvider.LevelConfig).AsSingle().NonLazy();
        Container.Bind<GameField>().FromInstance(_gameField).AsSingle().NonLazy();
        Container.Bind<GameLoop>().FromInstance(_gameLoop).AsSingle().NonLazy();
        Container.Bind<IScoreManager>().FromInstance(_scoreManager).AsSingle().NonLazy();

        #region Signals
        Container.DeclareSignal<SignalGameEnd>();
        Container.DeclareSignal<SignalInputChangeDirection>();

        Container.DeclareSignal<SignalInputOpenGameScreen>();
        Container.DeclareSignal<SignalInputOpenGamePopup>();

        Container.DeclareSignal<SignalAppleEated>();

        Container.DeclareSignal<SignalAddScore>();
        Container.DeclareSignal<SignalScoreChanged>();
        #endregion
    }
}
