using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameDataInstaller : MonoInstaller
    {
        [SerializeField] private LapRaceGameData _defaultLapRaceGameData;
        [SerializeField] private FreeRideGameData _defaultFreeRideGameData;

        public override void InstallBindings()
        {
            //For testing
            Container.Bind<LapRaceGameData>().FromInstance(_defaultLapRaceGameData).AsSingle().NonLazy();
            Container.Bind<FreeRideGameData>().FromInstance(_defaultFreeRideGameData).AsSingle().NonLazy();
            //

            Container.Bind<GameData>().FromInstance(new GameData(GameType.LapRace, _defaultLapRaceGameData)).AsSingle().NonLazy();
        }

        [Serializable]
        public class GameData
        {
            public GameData(GameType gameType, GameModeData gameModeData) : this(gameType)
            {
                this.gameModeData = gameModeData;
            }
            
            public GameData(GameType gameType)
            {
                this.gameType = gameType;
            }

            public GameType gameType;
            public GameModeData gameModeData;
        }

        public abstract class GameModeData
        {
            public string trackKey;

            public GameModeData() { }

            public GameModeData(string trackKey)
            {
                this.trackKey = trackKey;
            }
        }

        [Serializable]
        public class LapRaceGameData : GameModeData
        {
            public int lapCount;
            public int botCount;

            public LapRaceGameData() { }

            public LapRaceGameData(string trackKey, int lapCount, int botCount) : base (trackKey)
            {
                this.lapCount = lapCount;
                this.botCount = botCount;
            }
        }

        [Serializable]
        public class FreeRideGameData : GameModeData
        {
            
        }

        public enum GameType
        {
            LapRace,
            FreeRide
        }
    }
}