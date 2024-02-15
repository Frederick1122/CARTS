using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Installers;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "ModeConfig", menuName = "Configs/Mode Config")]
    public class ModeConfig : BaseConfig
    {
        //private const string DEFAULT_RACE_MODE = "Default race";
        //private const string FREE_RIDE_MODE = "Free ride";

        //private const string ONE_LAP_MODE = "One lap";
        //private const string THREE_LAP_MODE = "Three laps";

        //private const string ONE_BOT_MODE = "One bot";
        //private const string THREE_BOT_MODE = "Three bots";

        public Sprite Icon;

        [SerializeField] private GameDataInstaller.GameType _gameType;
        [SerializeField] private BotCount _botCount;
        [SerializeField] private LapCount _lapCount;

        public GameDataInstaller.GameType GetGameType()
        {
            return _gameType;
        }

        public int GetBotCountKey()
        {
            return _botCount switch
            {
                BotCount.One => 1,
                BotCount.Three => 3,
                _ => throw new System.NotImplementedException()
            };
        }

        public int GetLapCount()
        {
            return _lapCount switch
            {
                LapCount.One => 1,
                LapCount.Three => 3,
                _ => throw new System.NotImplementedException()
            };
        }
    }

    public enum BotCount
    {
        One = 0,
        Three = 1
    }

    public enum LapCount
    {
        One = 0,
        Three = 1
    }
}
