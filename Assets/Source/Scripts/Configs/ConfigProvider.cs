using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ConfigProvider : MonoBehaviour
    {
        public GraficConfig GraficConfig => _graficConfig;
        public GameplayConfig GameplayConfig => _gameplayConfig;
        public LevelConfig LevelConfig => GetLevelConfig(_loadType);

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

        private LevelConfig GetLevelConfig(ConfigLoadType loadType)
        {
            switch (loadType)
            {
                case ConfigLoadType.LevelList:
                    if(_currentLevelFromTransistor != null)
                    {
                        return _currentLevelFromTransistor;
                    }
                    else
                    {
                        Debug.LogWarning($"Level List Config is null, use fallback config instance {nameof(ConfigLoadType.DirectConfigJSON)}");
                        return GetLevelConfig(ConfigLoadType.DirectConfigJSON);
                    }
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
