using Base;
using System;
using System.Collections.Generic;

namespace Core.FSM
{
    public class FsmManager : Singleton<FsmManager>
    {
        private Dictionary<Type, Fsm> _currentFsms = new Dictionary<Type, Fsm>();

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

        public void AddNewFsm(Fsm newFsm)
        {
            if (_currentFsms.ContainsKey(newFsm.GetType()))
                return;

            newFsm.Init();
            _currentFsms.Add(newFsm.GetType(), newFsm);
        }

        public void RemoveFsm<T>() where T : Fsm
        {
            _currentFsms.Remove(typeof(T));
        }
    }
}