using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class KeyboardInputManager : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Update()
        {
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
    }
}
