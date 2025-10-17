using System;
using System.Collections.Generic;

namespace SanderSaveli.Snake
{
    public class FSM<TState> where TState : IFSMState
    {
        public TState ActiveState => _activeState;

        private Dictionary<Type, TState> _states = new();
        private TState _activeState;

        public void AddOrUpdateState<T>(T state) where T : TState
        {
            var type = typeof(T);
            _states[type] = state;
        }

        public void Update()
        {
            _activeState.OnUpdate();
        }

        public void ChangeState<T>() where T : TState
        {
            Type type = typeof(T);
            if (_states.TryGetValue(type, out var state))
                SetNewActiveState(state);
            else
                throw new KeyNotFoundException($"[FSM] State of type {type.Name} not found!");
        }


        private void SetNewActiveState(TState state)
        {
            _activeState?.OnExit();
            _activeState = state;
            _activeState.OnEnter();
        }
    }
}
