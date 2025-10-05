using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameLoop : MonoBehaviour
    {
        private class TickOrder
        {
            public TickableCellEntity Entity;
            public int Order;

            public TickOrder(TickableCellEntity entity, int order)
            {
                Entity = entity;
                Order = order;
            }
        }

        public float TickTime { get; private set; }

        private List<TickOrder> _tickEntities;
        private bool _isGameEnd;
        private Coroutine _loopRoutine;

        private void Awake()
        {
            _tickEntities = new List<TickOrder>();
        }

        [Inject]
        public void Construct(GameplayConfig gameplayConfig)
        {
            TickTime = gameplayConfig.TickTime;
        }

        public void StartGameLoop()
        {
            if (_loopRoutine == null)
            {
                _isGameEnd = false;
                _loopRoutine = StartCoroutine(TickLoop());
            }
        }

        public void EndGameLoop()
        {
            _isGameEnd = true;
            StopCoroutine(_loopRoutine);
            _loopRoutine = null;
        }

        public void SubscribeToTick(TickableCellEntity entity, int tickOrder)
        {
            _tickEntities.Add(new TickOrder(entity, tickOrder));
            _tickEntities.Sort((x, y) => x.Order - y.Order);
        }

        public void UnsubscribeFromTick(TickableCellEntity entity)
        {
            _tickEntities.Remove(_tickEntities.FirstOrDefault(t => t.Entity == entity));
        }

        private IEnumerator TickLoop()
        {
            while (!_isGameEnd)
            {
                yield return new WaitForSeconds(TickTime);
                Debug.Log("Tick!");
                Tick();
            }
        }

        private void Tick()
        {
            foreach (var entity in _tickEntities)
            {
                entity.Entity.Tick();
            }
        }
    }
}
