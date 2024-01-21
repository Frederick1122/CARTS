using ConfigScripts;

namespace Managers.Libraries
{
    public class CarLibrary : BaseLibrary<CarConfig>
    {
        private const string CAR_CONFIG_PATH = "Configs/Cars";

        protected override void Awake()
        {
            _path = CAR_CONFIG_PATH;
            base.Awake();
        }
    }
}