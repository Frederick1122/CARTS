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
        public AnimationCurve speedModifier;
        public AnimationCurve accelerationModifier;
        public AnimationCurve destroyTime;
        [Space]
        [Header("Map Pieces")]
        public MapPiecesHolder startPiece;
        public MapPiecesHolder[] piecePrefabs = new MapPiecesHolder[2];
        public int startCountOfPieces = 2;
    }
}