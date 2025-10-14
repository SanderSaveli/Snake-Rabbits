using SanderSaveli.UDK.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            #region Signals
            Container.DeclareSignal<SignalInputOpenMenuScreen>();
            Container.DeclareSignal<SignalInputOpenMenuPopup>();
            #endregion
        }
    }
}
