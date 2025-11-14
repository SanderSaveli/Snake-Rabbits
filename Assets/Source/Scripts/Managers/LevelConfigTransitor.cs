using UnityEngine;

namespace SanderSaveli.Snake
{
    public class LevelConfigTransitor : MonoBehaviour
    {
        public LevelConfig Config { get; private set; }

        public void SetLevel(int level_number)
        {
            Config = LevelConfigLoader.LoadConfig(level_number);
            Debug.Log("Config Loaded " + Config.level_number);
        }
    }
}
