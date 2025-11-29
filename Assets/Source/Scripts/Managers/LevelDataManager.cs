using Cysharp.Threading.Tasks;
using SanderSaveli.UDK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class LevelDataManager
    {
        public IReadOnlyList<LevelSaveData> Levels => _levels;
        public Action OnLevelDataUpdated { get; set; }
        public bool IsLoaded { get; private set; }

        private IStorageService _storageService;
        private List<LevelSaveData> _levels;
        private Dictionary<int, LevelData> _cachedData = new();

        public LevelDataManager(IStorageService storageService)
        {
            _storageService = storageService;
            Init();
        }

        private async void Init()
        {
            await UniTask.Yield();
            Debug.Log("Constructor");
            _storageService.Load<List<LevelSaveData>>(Const.LEVEL_PROGRESS_PATH, FillLevels);
        }

        public LevelData GetFullLevelData(LevelSaveData saveData)
        {
            if (_cachedData.ContainsKey(saveData.level_number))
            {
                return _cachedData[saveData.level_number];
            }
            LevelData levelData = CreateNewLevelData(saveData);
            _cachedData.Add(saveData.level_number, levelData);
            return levelData;
        }

        public void UpdateLevelData(LevelSaveData levelData)
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_levels[i].level_number == levelData.level_number)
                {
                    _levels[i] = levelData;
                    UnityEngine.Debug.Log("Data Updated!");
                    break;
                }
            }
            _storageService.Save(Const.LEVEL_PROGRESS_PATH, _levels, LogLevels);

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
            Debug.Log("Fill levels ");
            if (levelSaveDatas == null || levelSaveDatas.Count == 0)
            {
                Debug.LogWarning("Empty Level Save Data, Create new save");
                levelSaveDatas = InitLevels();
                _storageService.Save(Const.LEVEL_PROGRESS_PATH, levelSaveDatas);
            }
            Debug.Log("levels Count " + levelSaveDatas.Count);
            Debug.Log("ConfigsPaths: ");
            List<string> configs = LevelConfigLoader.GetAllConfigsPaths();
            Debug.Log("Paths count: " + configs.Count);
            if (levelSaveDatas.Count != configs.Count)
            {
                UnityEngine.Debug.LogWarning($"Discrepancy config count and save count. \n ConfigCount: {configs.Count} \n SaveCount: {levelSaveDatas.Count}");
                levelSaveDatas = InitLevels(levelSaveDatas, configs.Count);
                _storageService.Save(Const.LEVEL_PROGRESS_PATH, levelSaveDatas);
            }

            levelSaveDatas = levelSaveDatas.OrderBy(t => t.level_number).ToList();
            _levels = levelSaveDatas;
            IsLoaded = true;
            OnLevelDataUpdated?.Invoke();
            LogLevels();
        }

        private List<LevelSaveData> InitLevels()
        {
            List<LevelSaveData> levelSaveDatas = new List<LevelSaveData>();
            List<LevelConfig> levelConfig = LevelConfigLoader.LoadAllLevel();
            Debug.Log("Level Conigs count: " +  levelConfig.Count); 
            foreach (LevelConfig config in levelConfig)
            {
                levelSaveDatas.Add(new LevelSaveData(config.level_number, 0, 0, 0));
            }
            return levelSaveDatas;
        }

        private List<LevelSaveData> InitLevels(List<LevelSaveData> levelSaveDatas, int needLevelsData)
        {
            List<LevelSaveData> levels = new List<LevelSaveData>(levelSaveDatas);

            if (levels.Count < needLevelsData)
            {
                for (int i = levels.Count; i < needLevelsData; i++)
                {
                    levels.Add(new LevelSaveData(i + 1, 0, 0, 0));
                }
            }
            else if (levels.Count > needLevelsData)
            {
                levels.RemoveRange(needLevelsData - 1, levels.Count - needLevelsData);
            }

            return levels;
        }


        private LevelData CreateNewLevelData(LevelSaveData saveData)
        {
            try
            {
                LevelConfig levelConfig = LevelConfigLoader.LoadConfig(saveData.level_number);
                LevelData levelData = new LevelData(levelConfig, saveData);
                return levelData;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
                return null;
            }
        }

        private void LogLevels()
        {
            UnityEngine.Debug.Log("All levels Saves:");
            foreach (var level in _levels)
            {
                UnityEngine.Debug.Log($"Number: {level.level_number}\nScore: {level.max_score}\nStarCount: {level.star_count}");
            }
        }

        private void LogLevels(bool _)
        {
            OnLevelDataUpdated?.Invoke();
            LogLevels();
        }
    }
}
