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

            Container.Bind<GameData>().FromInstance(new GameData(GameType.LapRace)).AsSingle().NonLazy();
        }

        [Serializable]
        public class GameData
        {
            public GameData(GameType gameType)
            {
                this.gameType = gameType;
            }

            public GameType gameType;
            public GameModeData gameModeData;
        }

        public abstract class GameModeData { }

        [Serializable]
        public class LapRaceGameData : GameModeData
        {
            public string trackKey;
            public int lapCount;
            public int botCount;

            public LapRaceGameData() { }

            public LapRaceGameData(string trackKey, int lapCount, int botCount)
            {
                this.trackKey = trackKey;
                this.lapCount = lapCount;
                this.botCount = botCount;
            }
        }

        [Serializable]
        public class FreeRideGameData : GameModeData { }

        public enum GameType
        {
            LapRace,
            FreeRide
        }
    }
}