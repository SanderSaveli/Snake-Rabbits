using DG.Tweening;
using SanderSaveli.UDK;
using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class Apple : CellEntity, IPoolableObject<Apple>
    {
        [Tooltip("1 = TickTime, 0.5 = half of TickTime etc.")]
        [SerializeField] private float _animationTickTimeScale = 0.5f;
        public Action<Apple> OnBackToPool { get; set; }

        private SignalBus _signalBus;
        private float _animationDuretion;

        [Inject]
        public void Construct(SignalBus signalBus, GameplayConfig gameplayConfig)
        {
            _signalBus = signalBus;
            _animationDuretion = gameplayConfig.TickTime * _animationTickTimeScale;
        }

        private void OnDestroy()
        {
            DOTween.KillAll(transform);
        }

        public override void CollideWithHead(SnakeHead snake, out bool isKillSnake)
        {
            isKillSnake = false;
            snake.AddLength();
            _signalBus.Fire(new SignalAppleEated(CurrentCell, this));
            CurrentCell.SetEntity(null);
            PlayEatAnimation();
        }

        public override Type GetEntityType()
        {
            return typeof(Apple);
        }

        public void OnActive()
        {
            transform.localScale = Vector3.one;
        }

        public override void SetStartCell(Cell cell)
        {
            base.SetStartCell(cell);
            transform.position = cell.WorldPosition;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _animationDuretion);
        }

        private void PlayEatAnimation()
        {
            transform.DOScale(Vector3.zero, _animationDuretion)
                .OnComplete(() => OnBackToPool?.Invoke(this));
        }
    }
}
