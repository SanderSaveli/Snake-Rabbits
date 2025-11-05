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

        public LevelConfig()
        {
            field_width = 10; 
            field_height = 10;
            rabbit_positions = new List<Vector2Int>();
            carrot_positions = new List<Vector2Int>();
            obstacle_positions = new List<Vector2Int>();
        }

        public LevelConfig(int level_number, int field_width, int field_height, Vector2Int head_position, List<Vector2Int> rabbit_positions, List<Vector2Int> carrot_positions, List<Vector2Int> obstacle_positions, Direction start_direction, int start_segmets)
        {
            this.level_number = level_number;
            this.field_width = field_width;
            this.field_height = field_height;
            this.head_position = head_position;
            this.rabbit_positions = rabbit_positions;
            this.carrot_positions = carrot_positions;
            this.obstacle_positions = obstacle_positions;
            this.start_direction = start_direction;
            this.start_segmets = start_segmets;
        }
    }
}
