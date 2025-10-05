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
            config.fieldWidth = _fieldWidth;
            config.fieldHeight = _fieldHeight;
            config.headPosition = _headPosition;
            config.startDirection = _startDirection;
            config.startSegmets = _startSegmets;

            return config;
        }
    }
}
