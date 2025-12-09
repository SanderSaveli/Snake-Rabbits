using SanderSaveli.Snake;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private GameField _gameField;
    [SerializeField] private GameLoop _gameLoop;
    [SerializeField] private FieldPathFinder _pathFinder;
    [SerializeField] private ScoreTarget _scoreTarget;
    [SerializeField] private ScoreEffectShower _scoreEffectShowwer;
    [SerializeField] private WinScreen _winScreen;

    public override void InstallBindings()
    {
        Container.Bind<IGameField>().FromInstance(_gameField).AsSingle().NonLazy();
        Container.Bind<GameLoop>().FromInstance(_gameLoop).AsSingle().NonLazy();
        Container.Bind<IFieldPathFinder>().FromInstance(_pathFinder).AsSingle().NonLazy();
        Container.Bind<ScoreTarget>().FromInstance(_scoreTarget).AsSingle().NonLazy();
        Container.Bind<IScoreEffectShower>().FromInstance(_scoreEffectShowwer).AsSingle().NonLazy();
        Container.Bind<WinScreen>().FromInstance(_winScreen).AsSingle().NonLazy();

        #region Signals
        Container.DeclareSignal<SignalGameEnd>();
        Container.DeclareSignal<SignalGameStart>();
        Container.DeclareSignal<SignalGamePauseStatusChange>();
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
