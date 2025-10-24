using SanderSaveli.Snake;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private ConfigProvider _configProvider;
    [SerializeField] private GameField _gameField;
    [SerializeField] private GameLoop _gameLoop;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private FieldPathFinder _pathFinder;
    [SerializeField] private ScoreTarget _scoreTarget;
    [SerializeField] private ScoreEffectShower _scoreEffectShowwer;

    public override void InstallBindings()
    {
        Container.Bind<GameplayConfig>().FromInstance(_configProvider.GameplayConfig).AsSingle().NonLazy();
        Container.Bind<GraficConfig>().FromInstance(_configProvider.GraficConfig).AsSingle().NonLazy();
        Container.Bind<LevelConfig>().FromInstance(_configProvider.LevelConfig).AsSingle().NonLazy();
        Container.Bind<IGameField>().FromInstance(_gameField).AsSingle().NonLazy();
        Container.Bind<GameLoop>().FromInstance(_gameLoop).AsSingle().NonLazy();
        Container.Bind<IScoreManager>().FromInstance(_scoreManager).AsSingle().NonLazy();
        Container.Bind<IFieldPathFinder>().FromInstance(_pathFinder).AsSingle().NonLazy();
        Container.Bind<ScoreTarget>().FromInstance(_scoreTarget).AsSingle().NonLazy();
        Container.Bind<IScoreEffectShower>().FromInstance(_scoreEffectShowwer).AsSingle().NonLazy();

        #region Signals
        Container.DeclareSignal<SignalGameEnd>();
        Container.DeclareSignal<SignalGameStart>();
        Container.DeclareSignal<SignalDoPostGameAction>();
        Container.DeclareSignal<SignalInputChangeDirection>();

        Container.DeclareSignal<SignalInputOpenGameScreen>();
        Container.DeclareSignal<SignalInputOpenGamePopup>();

        Container.DeclareSignal<SignalAppleEated>();
        Container.DeclareSignal<SignalRabbitEated>();

        Container.DeclareSignal<SignalAddScore>();
        Container.DeclareSignal<SignalScoreChanged>();
        #endregion
    }
}
