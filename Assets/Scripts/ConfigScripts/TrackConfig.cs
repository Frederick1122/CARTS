using Race;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "Track Config", menuName = "Configs/Track Config")]
    public class TrackConfig : BaseConfig
    {
        public Track trackPrefab;
    }
}