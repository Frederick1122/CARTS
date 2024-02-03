using System;
using System.Collections.Generic;
using Base;
using Zenject;

namespace Race.RaceManagers
{
    public class RaceManager : Singleton<RaceManager>
    {
        [Inject] private DiContainer _diContainer;
        
        private Dictionary<RaceType, RaceState> _raceStates = new();
        private RaceState _currentRaceState;

        public RaceState GetState(RaceType state) 
        {
            return _raceStates[state];
        }
        
        public void SetState(RaceType state)
        {
            _currentRaceState = _raceStates[state];
        }

        public void Init()
        {
            _raceStates.Add(RaceType.LAP_RACE, _diContainer.Instantiate(typeof(LapRaceState)) as LapRaceState);
            _raceStates.Add(RaceType.FREE_RIDE, _diContainer.Instantiate(typeof(FreeRideState)) as FreeRideState);
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

        public void PauseRace() => _currentRaceState?.PauseRace();
    }


    public abstract class RaceState
    {
        public event Action OnFinishAction = delegate {  };
        public event Action OnStartAction = delegate {  };
        public event Action OnPauseAction = delegate {  };

        public abstract void Init();

        public abstract void Destroy();

        public virtual void StartRace() => OnStartAction?.Invoke();   

        public virtual void FinishRace() => OnFinishAction?.Invoke();

        public virtual void PauseRace() => OnPauseAction?.Invoke();
    }

    public enum RaceType
    {
        LAP_RACE,
        FREE_RIDE
    }
}