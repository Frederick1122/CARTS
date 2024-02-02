using UnityEngine;

namespace Cars.InputSystem.Player
{
    public abstract class PlayerInputSystem : MonoBehaviour, IInputSystem
    {
        public bool IsActive
        {
            get { return _inputHandler.IsActive; }
            set { _inputHandler.IsActive = value; }
        }

        public float VerticalInput => _vertInput;
        public float HorizontalInput => _horInput;
        public float BrakeInput => _handBrInput;

        protected float _vertInput = 0;
        protected float _horInput = 0;
        protected float _handBrInput = 0;

        protected IInputHandler _inputHandler;

        protected virtual void Awake()
        {
#if UNITY_ANDROID 
            _inputHandler = gameObject.AddComponent(typeof(MobileInputHandler)) as IInputHandler;
#else
            _inputHandler = gameObject.AddComponent(typeof(KeyBoardInputHandler)) as IInputHandler;
#endif

            _inputHandler.OnVerticalAxisChange += OnVerticalAxisChange;
            _inputHandler.OnHorizontalAxisChange += OnHorizontalAxisChange;
            _inputHandler.OnHandBrakeAxisChange += OnHandBrakeAxisChange;
        }


        protected virtual void OnDestroy()
        {
            if (_inputHandler == null)
                return;

            _inputHandler.OnVerticalAxisChange += OnVerticalAxisChange;
            _inputHandler.OnHorizontalAxisChange += OnHorizontalAxisChange;
            _inputHandler.OnHandBrakeAxisChange += OnHandBrakeAxisChange;
        }

        protected virtual void OnVerticalAxisChange(float value) =>
            _vertInput = value;

        protected virtual void OnHorizontalAxisChange(float value) =>
            _horInput = value;

        protected virtual void OnHandBrakeAxisChange(float value) =>
            _handBrInput = value;
    }
}
