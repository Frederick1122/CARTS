using Cars.InputSystem.Player;
using UnityEngine;

namespace UI.Windows.MobileLayout
{
    public class MobileLayoutView : UIView<MobileLayoutModel>, IButtonsInput
    {
        [field: SerializeField] public RaceButton ForwardButton { get; private set; }
        [field: SerializeField] public RaceButton BackwardButton { get; private set; }

        [field: SerializeField] public RaceButton RightButton { get; private set; }
        [field: SerializeField] public RaceButton LeftButton { get; private set; }

        [field: SerializeField] public RaceButton HandBrakeButton { get; private set; }
    }
}
