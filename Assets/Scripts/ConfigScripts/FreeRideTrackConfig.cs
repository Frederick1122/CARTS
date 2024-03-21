using FreeRide;
using FreeRide.Map;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "FreeRideTrackConfig", menuName = "Configs/Free Ride Track Config")]
    public class FreeRideTrackConfig : BaseConfig
    {
        public FreeRidePrefabData freeRidePrefab;
        
        [Header("Modifiers")]
        public FreeRideDifficultyModifierData freeRideDifficultyModifierData;
        [Space] 
        [Header("Map Fabric")] 
        public MapFabricData mapFabricData;
    }
}