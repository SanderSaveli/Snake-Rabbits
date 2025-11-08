using SanderSaveli.UDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SanderSaveli.Snake
{
    public class LevelDataManager
    {
        public IReadOnlyList<LevelSaveData> Levels => _levels;
        public Action OnAllLevelDataLoaded { get; set; }
        public bool IsLoaded { get; private set; }

        private IStorageService _storageService;
        private List<LevelSaveData> _levels;
        private Dictionary<int, LevelData> _cachedData = new();

        public LevelDataManager(IStorageService storageService)
        {
            _storageService = storageService;
            _storageService.Load<List<LevelSaveData>>(Const.LEVEL_PROGRESS_PATH, FillLevels);
        }

        public LevelData GetFullLevelData(LevelSaveData saveData)
        {
            if (_cachedData.ContainsKey(saveData.level_number))
            {
                return _cachedData[saveData.level_number];
            }
            return CreateNewLevelData(saveData);
        }

        public void UpdateLevelData(LevelSaveData levelData)
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_levels[i].level_number == levelData.level_number)
                {
                    _levels[i] = levelData;
                    break;
                }
            }
            _storageService.Save(Const.LEVEL_PROGRESS_PATH, _levels);

            if (_cachedData.ContainsKey(levelData.level_number))
            {
                _cachedData[levelData.level_number] = CreateNewLevelData(levelData);
            }
            else
            {
                _cachedData.Add(levelData.level_number, CreateNewLevelData(levelData));
            }
        }

        private void FillLevels(List<LevelSaveData> levelSaveDatas)
        {
            UnityEngine.Debug.Log("Levels loaded");
            if (levelSaveDatas == null || levelSaveDatas.Count == 0)
            {
                levelSaveDatas = InitLevels();
            }

            levelSaveDatas = levelSaveDatas.OrderBy(t => t.level_number).ToList();
            _levels = levelSaveDatas;
            IsLoaded = true;
            OnAllLevelDataLoaded?.Invoke();
        }

        private List<LevelSaveData> InitLevels()
        {
            List<LevelSaveData> levelSaveDatas = new List<LevelSaveData>();
            List<LevelConfig> levelConfig = LevelConfigLoader.LoadAllLevel();
            foreach (LevelConfig config in levelConfig)
            {
                levelSaveDatas.Add(new LevelSaveData(config.level_number, 0, 0, 0));
            }
            _storageService.Save(Const.LEVEL_PROGRESS_PATH, levelSaveDatas);
            return levelSaveDatas;
        }

        private LevelData CreateNewLevelData(LevelSaveData saveData)
        {
            try
            {
                LevelConfig levelConfig = LevelConfigLoader.LoadConfig(saveData.level_number);
                LevelData levelData = new LevelData(levelConfig, saveData);
                _cachedData.Add(saveData.level_number, levelData);
                return levelData;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
                return null;
            }
        }
    }
}
