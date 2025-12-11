using System;
namespace SanderSaveli.Snake
{
    public interface IHealthManager
    {
        public int Health { get; set; }

        public Action<int> OnHealthChange { get; set; }
        public Action OnDie {  get; set; }
    }
}
