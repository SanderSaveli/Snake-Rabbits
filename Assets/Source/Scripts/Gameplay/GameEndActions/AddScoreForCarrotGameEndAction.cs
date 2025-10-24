using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class AddScoreForCarrotGameEndAction : MonoBehaviour
    {
        [SerializeField] private float _timeForOneCarrot = 0.7f;

        private SignalBus _signalBus;
        private IGameField _gameField;
        private IScoreEffectShower _effectShower;

        [Inject]
        public void Construct(SignalBus signalBus, IGameField gameField, IScoreEffectShower effectShower)
        {
            _signalBus = signalBus;
            _gameField = gameField;
            _effectShower = effectShower;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalDoPostGameAction>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalDoPostGameAction>(HandleGameEnd);
        }

        private void HandleGameEnd(SignalDoPostGameAction action)
        {
            if (action.IsWin)
            {
                action.AddSubscriber(AddScoreForSaveCarrot(), 10);
            }
        }

        private async UniTask AddScoreForSaveCarrot()
        {
            List<Carrot> carrots = _gameField.FindAllEntity<Carrot>();
            int miliseconds = (int)(_timeForOneCarrot * 1000);
            await UniTask.Delay(1000);
            foreach (Carrot carrot in carrots)
            {
                _effectShower.ShowAndAddScore(20, carrot.transform.position);
                await UniTask.Delay(miliseconds);
            }
            await UniTask.Delay(1000);
        }
    }
}
