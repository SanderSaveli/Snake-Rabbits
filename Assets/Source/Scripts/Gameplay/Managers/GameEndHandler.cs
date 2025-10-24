using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private async void HandleGameEnd(SignalGameEnd ctx)
        {
            SignalDoPostGameAction doPostGameAction = new SignalDoPostGameAction(ctx.IsWin);
            _signalBus.Fire(doPostGameAction);
            List<UniTask> actions = new List<UniTask>(doPostGameAction.Subscribers);
            await HandleActions(actions);
            if (ctx.IsWin)
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Win));
            }
            else
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Lose));
            }
        }

        private async UniTask HandleActions(List<UniTask> actions)
        {
            foreach (UniTask action in actions)
            {
                await action;
            }
        }
    }
}
