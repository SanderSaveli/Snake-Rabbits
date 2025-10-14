using System;
using Zenject;

namespace SanderSaveli.Snake
{
    public class Carrot : Obstacle
    {
        public int TotalHealth { get; private set; }
        public int Health { get; private set; }

        public Action<int> OnHealthChange;
        public Action OnKill;

        [Inject]
        public void Construct(GameplayConfig gameplayConfig)
        {
            TotalHealth = gameplayConfig.CarrotHealth;
            Health = TotalHealth;
        }

        public override Type GetEntityType()
        {
            return typeof(Carrot);
        }

        public void TakeDamage()
        {
            Health -= 1;
            OnHealthChange?.Invoke(Health);

            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            CurrentCell.SetEntity(null);
            OnKill?.Invoke();
        }
    }
}
