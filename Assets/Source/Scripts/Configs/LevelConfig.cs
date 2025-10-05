using System;
using UnityEngine;

namespace SanderSaveli.Snake
{
    [Serializable]
    public struct LevelConfig
    {
        public int fieldWidth;
        public int fieldHeight;

        public Vector2Int headPosition;
        public Direction startDirection;
        public int startSegmets;
    }
}
