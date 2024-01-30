using ConfigScripts;

namespace Managers.Libraries
{
    public class FreeRideTrackLibrary : BaseLibrary<FreeRideTrackConfig>
    {
        private const string FREE_RIDE_TRACK_CONFIG_PATH = "Configs/FreeRideTracks";

        protected override void Awake()
        {
            _path = FREE_RIDE_TRACK_CONFIG_PATH;
            base.Awake();
        }
    }
}