using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SnakeHeadView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SnakeHead _head;
        [SerializeField] private SnakeMoveHandler _moveHandler;

        [Header("Params")]
        [SerializeField] private Direction _headSpriteDirection;

        private Vector3 _entityLayer;
        private float _tickTime;
        private float _initialAngel;
        private Tween _moveTween;
        private Tween _rotateTween;

        [Inject]
        public void Construct(GraficConfig graficConfig, GameplayConfig gameplayConfig)
        {
            _entityLayer = new Vector3(0, 0, graficConfig.EntityLayer);
            _tickTime = gameplayConfig.TickTime;
        }

        private void Start()
        {
            _moveTween = DOTween.Sequence(this);
            _initialAngel = GetInitialSpriteAngle();
            transform.position = _head.CurrentCell.View.transform.position + _entityLayer;
        }

        private void OnEnable()
        {
            _head.OnCellChange += HandleCellChanged;
            _moveHandler.OnNewDirectionInput += InputToCangeCell;
        }

        private void OnDisable()
        {
            _head.OnCellChange -= HandleCellChanged;
            _moveHandler.OnNewDirectionInput -= InputToCangeCell;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void HandleCellChanged(Cell newCell)
        {
            _moveTween?.Kill();
            Vector3 nextPosition = newCell.WorldPosition + _entityLayer;

            _moveTween = transform.DOMove(nextPosition, _tickTime).SetEase(Ease.Linear);
        }

        private void InputToCangeCell(Direction dir)
        {
            _rotateTween?.Kill();
            float angle = GetDirectionAndle(dir);
            _rotateTween = transform.DORotate(new Vector3(0, 0, angle), _tickTime / 2)
                .SetLink(gameObject);
        }

        private float GetInitialSpriteAngle()
        {
            switch (_headSpriteDirection)
            {
                case Direction.Up:
                    return 90;
                case Direction.Down:
                    return -90;
                case Direction.Left:
                    return 180;
                case Direction.Right:
                    return 0;
                default:
                    throw new System.NotImplementedException("There is no case for Direction " + _headSpriteDirection); ;
            }
        }

        private float GetDirectionAndle(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return 90 - _initialAngel;
                case Direction.Down:
                    return -90 - _initialAngel;
                case Direction.Left:
                    return -180 - _initialAngel;
                case Direction.Right:
                    return 0 - _initialAngel;
                default:
                    throw new System.NotImplementedException("There is no case for Direction " + _headSpriteDirection); ;
            }
        }
    }
}
