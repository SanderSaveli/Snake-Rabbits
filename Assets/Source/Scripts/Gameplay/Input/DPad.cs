using System;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class DPad : MonoBehaviour
    {
        public Action<Direction> OnInputDirection;

        [SerializeField] private DPadButton _topButton;
        [SerializeField] private DPadButton _rightButton;
        [SerializeField] private DPadButton _bottomButton;
        [SerializeField] private DPadButton _leftButton;

        private void OnEnable()
        {
            _topButton.Init(Direction.Up, OnButtonPressed);
            _rightButton.Init(Direction.Right, OnButtonPressed);
            _bottomButton.Init(Direction.Down, OnButtonPressed);
            _leftButton.Init(Direction.Left, OnButtonPressed);
        }

        private void OnButtonPressed(Direction dir)
        {
            OnInputDirection?.Invoke(dir);
        }
    }
}
