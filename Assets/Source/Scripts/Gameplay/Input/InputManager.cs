using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public enum InputType
    {
        Keyboard = 0,
        Dpad = 1,
    }
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputType _inputType;
        [SerializeField] private DPad _pad;
        private SignalBus _signalBus;
        private bool _isActive = true;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
#if !UNITY_EDITOR
            _inputType = InputType.Dpad;
#endif

            if (_inputType == InputType.Dpad)
            {
                _pad.OnInputDirection += HandleInput;
            }
            _signalBus.Subscribe<SignalGamePauseStatusChange>(HandlePauseStatusChange);
        }

        private void OnDisable()
        {
            if (_inputType == InputType.Dpad)
            {
                _pad.OnInputDirection -= HandleInput;
            }
            _signalBus.Unsubscribe<SignalGamePauseStatusChange>(HandlePauseStatusChange);
        }

        private void Update()
        {
            if(_inputType == InputType.Keyboard )
            {
                HandleKeyboardInput();
            }
        }

        private void HandleKeyboardInput()
        {
            if (!_isActive)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _signalBus.Fire(new SignalInputChangeDirection(Direction.Up));
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _signalBus.Fire(new SignalInputChangeDirection(Direction.Down));
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _signalBus.Fire(new SignalInputChangeDirection(Direction.Right));
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _signalBus.Fire(new SignalInputChangeDirection(Direction.Left));
            }
        }

        private void HandleInput(Direction direction)
        {
            if(!_isActive)
            {
                return;
            }
            _signalBus.Fire(new SignalInputChangeDirection(direction));
        }

        private void HandlePauseStatusChange(SignalGamePauseStatusChange ctx)
        {
            _isActive = !ctx.IsPause;
        }
    }
}
