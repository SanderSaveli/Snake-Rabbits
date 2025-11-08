using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private LevelConfigTransitor _configTransistor;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<ISceneLoader>().FromInstance(_sceneLoader).AsSingle().NonLazy();
            Container.Bind<DataManager>().FromInstance(_dataManager).AsSingle().NonLazy();
            Container.Bind<LevelConfigTransitor>().FromInstance(_configTransistor).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalInputClosePopup>();
            Container.DeclareSignal<SignalInputCloseScreen>();
            Container.DeclareSignal<SignalInputAction>();
            #endregion
        }
    }
}
