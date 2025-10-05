using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    [CreateAssetMenu(fileName = "new Gameplay Config", menuName = "Snake/Configs/Gameplay Config")]
    public class GameplayConfig : ScriptableObject
    {
        [Min(0)]
        [SerializeField] private float _ticksPerSecond = 1f;

        public float TickTime => 1 / _ticksPerSecond;
    }
}
