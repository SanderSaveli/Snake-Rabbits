using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SnakeHeadView : MonoBehaviour
    {
        [SerializeField] private SnakeHead _head;
        private Vector3 _entityLayer;
        private float _tickTime;

        [Inject]
        public void Construct(GraficConfig graficConfig, GameplayConfig gameplayConfig)
        {
            _entityLayer = new Vector3(0, 0, graficConfig.EntityLayer);
            _tickTime = gameplayConfig.TickTime;
        }

        private void Start()
        {
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
            Vector3 nextPosition = newCell.View.transform.position + _entityLayer;
            transform.DOMove(nextPosition, _tickTime).SetEase(Ease.Linear);
        }
    }
}
