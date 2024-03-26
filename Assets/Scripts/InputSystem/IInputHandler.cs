using System;

namespace Cars.InputSystem
{
    public interface IInputHandler
    {
        public event Action<float> OnVerticalAxisChange;
        public event Action<float> OnHorizontalAxisChange;

        public event Action<float> OnHandBrakeAxisChange;

        public bool IsActive { get; set; }

        public void Init();
    }
}
