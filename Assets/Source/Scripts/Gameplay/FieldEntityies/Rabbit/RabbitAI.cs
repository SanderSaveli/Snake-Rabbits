using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class RabbitAI : MonoBehaviour
    {
        [SerializeField] private Rabbit _rabbit;

        private FSM<IRabbitState> _fsm;
        private IFieldPathFinder _fieldPathFinder;
        private GameplayConfig _gameplayConfig;

        [Inject]
        public void Construct(IFieldPathFinder pathFinder, GameplayConfig gameplayConfig)
        {
            _fieldPathFinder = pathFinder;
            _gameplayConfig = gameplayConfig;
        }


        private void Awake()
        {
            IniFSM();
        }

        private void Start()
        {
            _fsm.ChangeState<MoveToCarrotState>();
        }

        public void Tick()
        {
            _fsm.Update();
        }

        private void IniFSM()
        {
            _fsm = new FSM<IRabbitState>();
            List<IRabbitState> states = new List<IRabbitState>()
            {
                new MoveToCarrotState(_fieldPathFinder, _gameplayConfig.RabbitConfig),
                new EatCarrotState()
            };

            foreach (var state in states)
            {
                state.Initialize(_rabbit, _fsm);
                _fsm.AddOrUpdateState(state);
            }
        }
    }
}
