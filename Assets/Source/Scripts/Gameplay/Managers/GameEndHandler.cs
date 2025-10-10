using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameEndHandler : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void HandleGameEnd(SignalGameEnd ctx)
        {
            if (ctx.IsWin)
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Win));
            }
            else
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Lose));
            }
        }
    }
}
