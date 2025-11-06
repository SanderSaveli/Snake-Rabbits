using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    [Serializable]
    public class LevelConfig
    {
        public int level_number;

        public int score_for_first_star;
        public int score_for_second_star;
        public int score_for_third_star;

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

            score_for_first_star = 100;
            score_for_second_star = 200;
            score_for_third_star = 300;
            rabbit_positions = new List<Vector2Int>();
            carrot_positions = new List<Vector2Int>();
            obstacle_positions = new List<Vector2Int>();
        }
    }
}
