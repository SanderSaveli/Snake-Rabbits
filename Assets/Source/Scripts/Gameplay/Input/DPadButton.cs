using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SanderSaveli.Snake
{
    public class DPadButton : MonoBehaviour, IPointerDownHandler
    {
        private Direction _direction;
        private Action<Direction> _callback;

        public void Init(Direction direction, Action<Direction> callback)
        {
            _direction = direction;
            _callback = callback;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _callback?.Invoke(_direction);
        }
    }
}
