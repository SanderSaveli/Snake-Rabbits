using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class AllRabbitsEatedGameEndCondition : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IGameField _gameField;

        [Inject]
        public void Construct(SignalBus signalBus, IGameField gameField)
        {
            _signalBus = signalBus;
            _gameField = gameField;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalRabbitEated>(HandleEatRabbit);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalRabbitEated>(HandleEatRabbit);
        }

        private void HandleEatRabbit(SignalRabbitEated ctx)
        {
            List<Cell> rabbits = _gameField.FindAllCellsWithEntity<Rabbit>();

            if (rabbits == null || rabbits.Count == 0)
            {
                _signalBus.Fire(new SignalGameEnd(GameEndStatus.Win));
            }
        }
    }
}
