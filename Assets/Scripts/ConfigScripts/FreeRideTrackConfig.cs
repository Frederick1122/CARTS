using FreeRide;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "FreeRideTrackConfig", menuName = "Configs/Free Ride Track Config")]
    public class FreeRideTrackConfig : BaseConfig
    {
        public FreeRidePrefabData freeRidePrefab;
    }
}