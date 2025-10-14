using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<ISceneLoader>().FromInstance(_sceneLoader).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalInputClosePopup>();
            Container.DeclareSignal<SignalInputCloseScreen>();
            Container.DeclareSignal<SignalInputAction>();
            #endregion
        }
    }
}
