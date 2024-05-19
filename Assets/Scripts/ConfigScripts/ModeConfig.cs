using System.Collections.Generic;
using UnityEngine;
using Installers;
using Knot.Localization;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "ModeConfig", menuName = "Configs/Mode Config")]
    public class ModeConfig : BaseConfig
    {
        public Sprite Icon;
        [SerializeField] private KnotTextKeyReference _localizedName;
        [SerializeField] private GameDataInstaller.GameType _gameType;
        [SerializeField] private BotCount _botCount;
        [SerializeField] private LapCount _lapCount;
        [SerializeField] private Rarity _carClass;
        [SerializeField] private List<BaseTrackConfig> _maps = new();

        public IReadOnlyList<BaseTrackConfig> Maps { get {  return _maps; } }

        public GameDataInstaller.GameType GetGameType()
        {
            return _gameType;
        }

        public string GetLocalizedName()
        {
            return _localizedName.Value;
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

        public Rarity GetCarClass() { return _carClass; }
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
