using Base;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.FSM
{
    public class FsmManager : Singleton<FsmManager>
    {
        [Inject] private DiContainer _diContainer;
        
        private Dictionary<Type, Fsm> _currentFsms = new ();
        private Fsm _activeFsm;
        
        protected override void Awake()
        {
            base.Awake();
            foreach (var currentFsm in _currentFsms)
                currentFsm.Value.Init();
        }

        public void SetActiveFsm<T>() where T : Fsm, new()
        {
            if (!_currentFsms.ContainsKey(typeof(T)))
            {
                var newFsm = _diContainer.Instantiate(typeof(T)) as T;
                newFsm.Init();
                _currentFsms.Add(typeof(T), newFsm);
            }
            
            _activeFsm?.Reset();
            _activeFsm = _currentFsms[typeof(T)];
            _activeFsm.SetStartState();
        }

        public Fsm TryGetFsm<T>() where T : Fsm
        {
            AddNewFsm<Fsm>();
            return _currentFsms.TryGetValue(typeof(T), out var fsm) ? fsm : null;
        }

        public void AddNewFsm<T>() where T : Fsm, new()
        {
            if (_currentFsms.ContainsKey(typeof(T)))
                return;

            var newFsm = new T();
            newFsm.Init();
            _currentFsms.Add(typeof(T), newFsm);
        }

        public void AddNewFsm(IFsm newFsm)
        {
            if (_currentFsms.ContainsKey(newFsm.GetType()))
                return;

            newFsm.Init();
            _currentFsms.Add(newFsm.GetType(), (Fsm)newFsm);
        }

        public void RemoveFsm<T>() where T : Fsm
        {
            _currentFsms.Remove(typeof(T));
        }

        public void RemoveFsm(IFsm fsm)
        {
            _currentFsms.Remove(fsm.GetType());
        }
    }
}