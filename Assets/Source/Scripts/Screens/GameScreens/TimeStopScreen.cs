using SanderSaveli.UDK.UI;
using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class TimeStopScreen : UiScreen
    {
        protected SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Show(Action callback = null)
        {
            Time.timeScale = 0;
            _signalBus.Fire(new SignalGamePauseStatusChange(true));
            base.Show(callback);
        }

        public override void Hide(Action callback = null)
        {
            Time.timeScale = 1;
            _signalBus.Fire(new SignalGamePauseStatusChange(false));
            base.Hide(callback);
        }

        public override void ShowImmediately()
        {
            Time.timeScale = 0;
            base.ShowImmediately();
        }

        public override void HideImmediately()
        {
            Time.timeScale = 1;
            base.HideImmediately();
        }
    }
}
