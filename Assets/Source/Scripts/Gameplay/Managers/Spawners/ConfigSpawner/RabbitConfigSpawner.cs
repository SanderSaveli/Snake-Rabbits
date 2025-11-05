using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class RabbitConfigSpawner : ConfigSpawner<Rabbit>
    {
        protected override List<Vector2Int> GetAllEntityCoordinate(LevelConfig levelConfig) =>
            levelConfig.rabbit_positions;
    }
}
