using ConfigScripts;

namespace Managers.Libraries
{
    public class TrackLibrary : BaseLibrary<BaseTrackConfig>
    {
        private const string TRACK_CONFIG_PATH = "Configs/Tracks";
        private const string FREE_RIDE_TRACK_CONFIG_PATH = "Configs/FreeRideTracks";

        protected override void Awake()
        {
            _paths.Add(FREE_RIDE_TRACK_CONFIG_PATH);
            _paths.Add(TRACK_CONFIG_PATH);
            base.Awake();
        }
    }
}