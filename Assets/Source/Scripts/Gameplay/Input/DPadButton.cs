using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    [RequireComponent(typeof(Image))]
    public class DPadButton : MonoBehaviour, IPointerDownHandler
    {
        private Direction _direction;
        private Action<Direction> _callback;

        void Awake()
        {
            var img = GetComponent<Image>();
            img.alphaHitTestMinimumThreshold = 0.1f; 
        }

        public void Init(Direction direction, Action<Direction> callback)
        {
            _direction = direction;
            _callback = callback;
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            _callback?.Invoke(_direction);
        }
    }
}
