using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SnakeTailView : MonoBehaviour
    {
        [SerializeField] private SnakeTail _tail;
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
            transform.position = _tail.CurrentCell.View.transform.position + _entityLayer;
        }

        private void OnEnable()
        {
            _tail.OnChangeCell += HandleCellChanged;
        }

        private void OnDisable()
        {
            _tail.OnChangeCell -= HandleCellChanged;
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
