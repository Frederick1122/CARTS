using UnityEngine;

namespace UI.Windows.RaceLoadOut
{
    public class RaceLoadOutView : UIView<RaceLoadOutModel>, IButtonsInput
    {
        [field: SerializeField] public RaceButton RightButton { get; private set; }
        [field: SerializeField] public RaceButton LeftButton { get; private set; }
        [field: SerializeField] public RaceButton BrakeButton { get; private set; }
    }
}
