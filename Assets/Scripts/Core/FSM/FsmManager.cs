using System;
using System.Collections.Generic;
using Base;

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
            
            _currentFsms.Add(typeof(T), new T()); 
        }

        public void RemoveFsm<T>() where T : Fsm
        {
            _currentFsms.Remove(typeof(T));
        }
    }
}