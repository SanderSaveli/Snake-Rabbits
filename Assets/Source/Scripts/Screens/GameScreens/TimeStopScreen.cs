using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SanderSaveli.UDK.UI;
using System;

namespace SanderSaveli.Snake
{
    public class TimeStopScreen : UiScreen
    {
        public override void Show(Action callback = null)
        {
            Time.timeScale = 0;
            base.Show(callback);
        }

        public override void Hide(Action callback = null)
        {
            Time.timeScale = 1;
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
