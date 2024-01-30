using System;
using System.Collections.Generic;
using Base;
using Zenject;

namespace Race.RaceManagers
{
    public class RaceManager : Singleton<RaceManager>
    {
        [Inject] private DiContainer _diContainer;
        
        private Dictionary<Type, RaceState> _raceStates = new();
        private RaceState _currentRaceState;

        public T GetState<T>() where T : RaceState
        {
            return (T)_raceStates[typeof(T)];
        }
        
        public void SetState<T>() where T : RaceState
        {
            _currentRaceState = _raceStates[typeof(T)];
        }

        public void Init()
        {
            _raceStates.Add(typeof(LapRaceState), _diContainer.Instantiate(typeof(LapRaceState)) as LapRaceState);
            _raceStates.Add(typeof(FreeRideState), _diContainer.Instantiate(typeof(FreeRideState)) as FreeRideState);
        }

        public void OnDestroy()
        {
            foreach (var state in _raceStates) 
                state.Value.Destroy();
        }

        public void InitState() => _currentRaceState?.Init();

        public void DestroyRace() => _currentRaceState?.Destroy();

        public void StartRace() => _currentRaceState?.StartRace();

        public void FinishRace() => _currentRaceState?.FinishRace();
    }


    public abstract class RaceState
    {
        public event Action OnFinishAction = delegate {  };
        public event Action OnStartAction = delegate {  };
        
        public abstract void Init();

        public abstract void Destroy();

        public virtual void StartRace()
        {
            OnStartAction?.Invoke();   
        }

        public virtual void FinishRace()
        {
            OnFinishAction?.Invoke();
        }
    }
}