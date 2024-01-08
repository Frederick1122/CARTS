using System.Collections.Generic;
using Installers;
using Managers;
using UnityEngine;
using Zenject;

namespace Race
{
    public class LapRaceManager : RaceManager
    {
        [Inject] private TrackData _trackData;
    
        private Track _currentTrack;
        private List<CarController> _enemies = new();
    
        public override void Init()
        {
            InitTrack();
            base.Init();
            InitAi();
        }
    
        public override void StartRace()
        {
            foreach (var en in _enemies)
                en.StartCar();
    
            _player.StartCar();
        }
    
        [ContextMenu("Test Race")]
        public void TestRace()
        {
            Init();
            StartRace();
        }
    
        protected override void InitPlayer()
        {
            _player = _currentTrack.SpawnPlayer(_playerPrefab);
        }
    
        private void InitAi()
        {
            _enemies = _currentTrack.SpawnAiTrucks();
        }
    
        private void InitTrack()
        {
            var trackConfig = TrackLibrary.Instance.GetConfig(_trackData.configKey);
            _currentTrack = Instantiate(trackConfig.trackPrefab);
        }
    }
}

