using UnityEngine;

namespace SanderSaveli.Snake
{
    [CreateAssetMenu(fileName = "new Rabbit Config", menuName = "Snake/Configs/Rabbit config")]
    public class RabbitConfig : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int _moveDuration;
        [Min(1)]
        [SerializeField] private int _rotateDuration;

        public int MoveDuration => _moveDuration;
        public int RotateDuration => _rotateDuration;
    }
}
