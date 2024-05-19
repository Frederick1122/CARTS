using ConfigScripts;

namespace Managers.Libraries
{
    public class PresetLibrary : BaseLibrary<CarPresetConfig>
    {
        private const string CAR_PRESET_CONFIG_PATH = "Configs/CarPresets";

        protected override void Awake()
        {
            _paths.Add(CAR_PRESET_CONFIG_PATH);
            base.Awake();
        }
    }
}