using System;
using UI;
using UI.Windows.MobileLayout;
using UnityEngine;

namespace Cars.InputSystem.Player
{
    public class MobileInputHandler : MonoBehaviour, IInputHandler
    {
        public bool IsActive { get; set; } = false;

        public event Action<float> OnVerticalAxisChange;
        public event Action<float> OnHorizontalAxisChange;
        public event Action<float> OnHandBrakeAxisChange;

        private IButtonsInput _buttonController;

        public void Init()
        {
            var controller = UIManager.Instance.GetMobileLayoutUI().ShowWindow(typeof(MobileLayoutController), false)
                as MobileLayoutController;
            Sub(controller.GetButtons());
        }

        public void Sub(IButtonsInput buttonController)
        {
            _buttonController = buttonController;
            _buttonController.ForwardButton.OnDown += Forward;
            _buttonController.ForwardButton.OnUp += StopMoving;

            _buttonController.BackwardButton.OnDown += Backward;
            _buttonController.BackwardButton.OnUp += StopMoving;

            _buttonController.LeftButton.OnDown += TurnLeft;
            _buttonController.LeftButton.OnUp += StopTurn;

            _buttonController.RightButton.OnDown += TurnRight;
            _buttonController.RightButton.OnUp += StopTurn;

            _buttonController.HandBrakeButton.OnDown += HandBrake;
            _buttonController.HandBrakeButton.OnUp += StopHandBrake;
        }

        private void OnDestroy()
        {
            if (_buttonController == null)
                return;

            _buttonController.ForwardButton.OnDown -= Forward;
            _buttonController.ForwardButton.OnUp -= StopMoving;

            _buttonController.BackwardButton.OnDown -= Backward;
            _buttonController.BackwardButton.OnUp -= StopMoving;

            _buttonController.LeftButton.OnDown -= TurnLeft;
            _buttonController.LeftButton.OnUp -= StopTurn;

            _buttonController.RightButton.OnDown -= TurnRight;
            _buttonController.RightButton.OnUp -= StopTurn;

            _buttonController.HandBrakeButton.OnDown -= HandBrake;
            _buttonController.HandBrakeButton.OnUp -= StopHandBrake;
        }

        public void TurnRight() =>
            OnHorizontalAxisChange?.Invoke(1);

        public void TurnLeft() =>
            OnHorizontalAxisChange?.Invoke(-1);

        public void StopTurn() =>
            OnHorizontalAxisChange?.Invoke(0);

        public void Forward() =>
            OnVerticalAxisChange?.Invoke(1);

        public void Backward() =>
            OnVerticalAxisChange?.Invoke(-1);

        public void StopMoving() =>
            OnVerticalAxisChange?.Invoke(1);

        public void HandBrake() =>
            OnHandBrakeAxisChange?.Invoke(1);

        public void StopHandBrake() =>
            OnHandBrakeAxisChange?.Invoke(0);
    }
}
