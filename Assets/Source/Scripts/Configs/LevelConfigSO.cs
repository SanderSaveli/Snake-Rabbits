using UnityEngine;

namespace SanderSaveli.Snake
{
    [CreateAssetMenu(fileName = "new Level Config", menuName = "Snake/Level Config")]
    public class LevelConfigSO : ScriptableObject
    {
        [SerializeField] private int _fieldWidth;
        [SerializeField] private int _fieldHeight;

        [SerializeField] private Vector2Int _headPosition;
        [SerializeField] private Direction _startDirection;
        [SerializeField] private int _startSegmets;

        public int FieldWidth => _fieldWidth;
        public int FieldHeight => _fieldHeight;

        public Vector2Int HeadPosition => _headPosition;
        public Direction StartDirection => StartDirection;
        public int StartSegmets => _startSegmets;

        public LevelConfig ToConfig()
        {
            LevelConfig config = new LevelConfig();
            config.field_width = _fieldWidth;
            config.field_height = _fieldHeight;
            config.head_position = _headPosition;
            config.start_direction = _startDirection;
            config.start_segmets = _startSegmets;

            return config;
        }
    }
}
