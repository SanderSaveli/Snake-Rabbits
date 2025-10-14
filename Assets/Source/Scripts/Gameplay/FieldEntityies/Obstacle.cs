using SanderSaveli.UDK;
using System;

namespace SanderSaveli.Snake
{
    public class Obstacle : CellEntity, IPoolableObject<Obstacle>
    {
        public Action<Obstacle> OnBackToPool { get; set; }

        public override void CollideWithHead(SnakeHead snake, out bool isKillSnake)
        {
            isKillSnake = true;
            snake.Die();
        }

        public override Type GetEntityType() => typeof(Obstacle);

        public void OnActive()
        {
            
        }
    }
}
