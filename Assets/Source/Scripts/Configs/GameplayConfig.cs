using UnityEngine;

namespace SanderSaveli.Snake
{
    [CreateAssetMenu(fileName = "new Gameplay Config", menuName = "Snake/Configs/Gameplay Config")]
    public class GameplayConfig : ScriptableObject
    {
        [Min(0)]
        [SerializeField] private float _ticksPerSecond = 1f;
        [Min(0)]
        [SerializeField] private float _gameDuration = 30f;

        [Space]
        [SerializeField] private int _carrotHealth = 40;

        [Space]
        [Range(0, 1)]
        [Header("Snake")]
        [SerializeField] private float _coyoteTimeRelativeToTick = 0.2f;

        [Space]
        [Header("Rabbit")]
        [SerializeField] private RabbitConfig _rabbitConfig;

        public float TickTime => 1 / _ticksPerSecond;
        public float GameDuration => _gameDuration;
        public int CarrotHealth => _carrotHealth;
        public float CoyoteTime => TickTime * _coyoteTimeRelativeToTick;
        public RabbitConfig RabbitConfig => _rabbitConfig;
    }
}
