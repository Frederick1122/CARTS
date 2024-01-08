using Race;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "RaceConfig", menuName = "Configs/Race Config")]
    public class TrackConfig : BaseConfig
    {
        public Track trackPrefab;
    }
}