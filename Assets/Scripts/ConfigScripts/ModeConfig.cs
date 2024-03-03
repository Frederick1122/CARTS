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
        public Sprite Icon;

        [SerializeField] private GameDataInstaller.GameType _gameType;
        [SerializeField] private BotCount _botCount;
        [SerializeField] private LapCount _lapCount;
        [SerializeField] private CarClass _carClass;
        [SerializeField] private List<BaseConfig> _maps = new();

        public IReadOnlyList<BaseConfig> Maps { get {  return _maps; } }

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

        public CarClass GetCarClass() { return _carClass; }
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
