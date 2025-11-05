using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class CarrotConfigSpawner : ConfigSpawner<Carrot>
    {
        protected override List<Vector2Int> GetAllEntityCoordinate(LevelConfig levelConfig) =>
            levelConfig.carrot_positions;
    }
}
