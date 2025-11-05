using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ObstacleConfigSpawner : ConfigSpawner<Obstacle>
    {
        protected override List<Vector2Int> GetAllEntityCoordinate(LevelConfig levelConfig) =>
            levelConfig.obstacle_positions;
    }
}
