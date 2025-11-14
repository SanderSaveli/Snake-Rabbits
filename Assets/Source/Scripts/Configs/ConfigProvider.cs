using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ConfigProvider : MonoBehaviour
    {
        public GraficConfig GraficConfig => _graficConfig;
        public GameplayConfig GameplayConfig => _gameplayConfig;
        public LevelConfig LevelConfig => GetLevelConfig();

        [SerializeField] private ConfigLoadType _loadType;

        [SerializeField] private GraficConfig _graficConfig;
        [SerializeField] private GameplayConfig _gameplayConfig;

        [Header("Configs")]
        [SerializeField] private LevelConfigSO _levelConfig;
        [SerializeField] private TextAsset _levelJSON;
        private LevelConfig _currentLevelFromTransistor;


        [Inject]
        public void Construct(LevelConfigTransitor levelConfigTransitor)
        {
            _currentLevelFromTransistor = levelConfigTransitor.Config;
        }

        private LevelConfig GetLevelConfig()
        {
            switch (_loadType)
            {
                case ConfigLoadType.LevelList:
                    return _currentLevelFromTransistor;
                case ConfigLoadType.DirectConfigSO:
                    return _levelConfig.ToConfig();
                case ConfigLoadType.DirectConfigJSON:
                    return JsonConvert.DeserializeObject<LevelConfig>(_levelJSON.text);
                default:
                    throw new System.NotImplementedException($"There is no case for {nameof(ConfigLoadType)}= {_loadType}");
            }
        }
    }
}
