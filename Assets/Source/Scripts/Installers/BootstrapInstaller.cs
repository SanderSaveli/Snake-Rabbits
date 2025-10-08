using SanderSaveli.UDK.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            #region Signals
            Container.DeclareSignal<SignalInputClosePopup>();
            #endregion
        }
    }
}
