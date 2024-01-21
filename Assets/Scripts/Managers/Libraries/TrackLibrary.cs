using ConfigScripts;

namespace Managers.Libraries
{
    public class TrackLibrary : BaseLibrary<TrackConfig>
    {
        private const string TRACK_CONFIG_PATH = "Configs/Tracks";

        protected override void Awake()
        {
            _path = TRACK_CONFIG_PATH;
            base.Awake();
        }   
    }
}