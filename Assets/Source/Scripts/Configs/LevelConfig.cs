using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    [Serializable]
    public class LevelConfig
    {
        public int level_number;
        public int field_width;
        public int field_height;

        public Vector2Int head_position;
        public List<Vector2Int> rabbit_positions;
        public List<Vector2Int> carrot_positions;
        public List<Vector2Int> obstacle_positions;
        public Direction start_direction;
        public int start_segmets;
    }
}
