using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public abstract class GameEndHandler : MonoBehaviour
    {
        protected SignalBus _signalBus;

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

        private async void HandleGameEnd(SignalGameEnd ctx)
        {
            _signalBus.Fire(new SignalGamePauseStatusChange(true)); 
            SignalDoPostGameAction doPostGameAction = new SignalDoPostGameAction(ctx.IsWin);
            _signalBus.Fire(doPostGameAction);
            List<UniTask> actions = new List<UniTask>(doPostGameAction.Subscribers);
            await HandleActions(actions);

            if (ctx.IsWin)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }

        public abstract void Win();

        public abstract void Lose();

        private async UniTask HandleActions(List<UniTask> actions)
        {
            foreach (UniTask action in actions)
            {
                await action;
            }
        }
    }
}
