using DG.Tweening;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SnakeHeadView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SnakeHead _head;

        [Header("Params")]
        [SerializeField] private Direction _headSpriteDirection;

        private Vector3 _entityLayer;
        private float _tickTime;
        private Cell _previousCell;
        private float _initialAngel;

        [Inject]
        public void Construct(GraficConfig graficConfig, GameplayConfig gameplayConfig)
        {
            _entityLayer = new Vector3(0, 0, graficConfig.EntityLayer);
            _tickTime = gameplayConfig.TickTime;
        }

        private void Start()
        {
            _initialAngel = GetInitialSpriteAngle();
            transform.position = _head.CurrentCell.View.transform.position + _entityLayer;
        }

        private void OnEnable()
        {
            _head.OnCellChange += HandleCellChanged;
        }

        private void OnDisable()
        {
            _head.OnCellChange -= HandleCellChanged;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void HandleCellChanged(Cell newCell)
        {
            _previousCell = newCell;
            Vector3 nextPosition = newCell.WorldPosition + _entityLayer;
            transform.DOMove(nextPosition, _tickTime).SetEase(Ease.Linear);
            Vector3 direction = nextPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle -= _initialAngel;

            transform.DORotate(new Vector3(0, 0, angle), _tickTime /2);
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
    }
}
