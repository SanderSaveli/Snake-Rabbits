using System;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ArcadeHealthManager : MonoBehaviour, IHealthManager
    {
        public int Health
        {
            get => _health; 
            set
            {
                _health = Math.Clamp(value, 0 , int.MaxValue);
                OnHealthChange?.Invoke(_health);
                if(_health == 0 )
                {
                    OnDie?.Invoke();
                }
            }
        }

        public Action<int> OnHealthChange { get; set; }
        public Action OnDie { get; set; }
        private int _health;

        private void Start()
        {
            ArcadeContext context = ArcadeContext.Instance;
            Health = context.Health;
        }
    }
}
