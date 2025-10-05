using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ConfigProvider : MonoBehaviour
    {
        public GraficConfig GraficConfig => _graficConfig;
        public GameplayConfig GameplayConfig => _gameplayConfig;
        public LevelConfig LevelConfig => _levelConfig.ToConfig();

        [SerializeField] private GraficConfig _graficConfig;
        [SerializeField] private GameplayConfig _gameplayConfig;
        [SerializeField] private LevelConfigSO _levelConfig;
    }
}
