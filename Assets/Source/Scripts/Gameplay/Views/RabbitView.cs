using DG.Tweening;
using System.Threading;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class RabbitView : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private Direction _spriteDirection;

        [SerializeField] private Rabbit _rabbit;
        private float _tickTime;
        private Vector3 _entityLayer;
        private float _initialAngel;

        [Inject]
        public void Construct(GameplayConfig gameplayConfig, GraficConfig graficConfig)
        {
            _tickTime = gameplayConfig.TickTime;
            _entityLayer = new Vector3(0, 0, graficConfig.EntityLayer);
        }

        private void Start()
        {
            _initialAngel = GetInitialSpriteAngle();
        }

        private void OnEnable()
        {
            _rabbit.OnCellChange += HandleChangeCell;
            _rabbit.OnRotate += HandleRotate;
        }

        private void OnDisable()
        {
            _rabbit.OnCellChange -= HandleChangeCell;
            _rabbit.OnRotate -= HandleRotate;
        }

        private void HandleChangeCell(Cell cell)
        {
            Vector3 nextPosition = cell.WorldPosition + _entityLayer;
            transform.DOMove(nextPosition, _tickTime).SetEase(Ease.Linear);
        }

        private void HandleRotate(Direction direction)
        {
            Vector2Int rotate =  DirectionTool.DirectionToVector2(direction);

            float angle = Mathf.Atan2(rotate.y, rotate.x) * Mathf.Rad2Deg;

            angle -= _initialAngel;

            transform.DORotate(new Vector3(0, 0, angle), _tickTime / 2);
        }

        private float GetInitialSpriteAngle()
        {
            switch (_spriteDirection)
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
                    throw new System.NotImplementedException("There is no case for Direction " + _spriteDirection); ;
            }
        }
    }
}
