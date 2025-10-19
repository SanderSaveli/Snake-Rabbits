using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class RabbitTestInstaller : MonoInstaller
    {
        [SerializeField] private ConfigProvider _configProvider;
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameLoop _gameLoop;
        [SerializeField] private FieldPathFinder _pathFinder;

        public override void InstallBindings()
        {
            Container.Bind<GameplayConfig>().FromInstance(_configProvider.GameplayConfig).AsSingle().NonLazy();
            Container.Bind<GraficConfig>().FromInstance(_configProvider.GraficConfig).AsSingle().NonLazy();
            Container.Bind<LevelConfig>().FromInstance(_configProvider.LevelConfig).AsSingle().NonLazy();
            Container.Bind<IGameField>().FromInstance(_gameField).AsSingle().NonLazy();
            Container.Bind<GameLoop>().FromInstance(_gameLoop).AsSingle().NonLazy();
            Container.Bind<IFieldPathFinder>().FromInstance(_pathFinder).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalGameEnd>();
            #endregion
        }
    }
}
