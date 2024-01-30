using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.FSM
{
    public class Fsm : IFsm
    {
        private FsmState _currentState;
        protected Dictionary<Type, FsmState> _states = new Dictionary<Type, FsmState>();

        public virtual void Init()
        {
        }

        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);

            if (_states.TryGetValue(type, out var newState))
            {
                SetState(newState);
            }
            else
            {
                Debug.LogError($"{this} : FSM not found state - {type}");
            }
        }

        public void SetState(FsmState state)
        {
            if (_currentState == state)
                return;

            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void SetStartState()
        {
            SetState(_states.First().Value);
        }

        public void Reset()
        {
            _currentState?.Exit();
            _currentState = null;
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }

    public interface IFsm
    {
        public void Init()
        {
        }

        public void SetState<T>() where T : FsmState
        {
        }

        public void SetStartState();

        public void Reset();
    }
}