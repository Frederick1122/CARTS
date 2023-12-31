using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FSM
{
    public class Fsm
    {
        private FsmState _currentState;
        private Dictionary<Type, FsmState> _states = new Dictionary<Type, FsmState>();

        public Fsm()
        {
            Init();
        }
        
        protected virtual void Init() { }
        
        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);
            
            if (_currentState.GetType() == type)
                return;

            if (_states.TryGetValue(type, out var newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }
            else
            {
                Debug.LogAssertion($"{GetType()} not found state {type}");
            }
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}