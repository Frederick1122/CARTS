using Cars.Controllers;
using UnityEngine;

namespace ConfigScripts
{
    [CreateAssetMenu(fileName = "CarPresetConfig", menuName = "Configs/Car Preset Config")]
    public class CarPresetConfig : BaseConfig
    {
        [Header("Movement Type")]
        public MovementMode movementMode;
        public GroundCheck groundCheck;
        public LayerMask drivableSurface;
    }
}