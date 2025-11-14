using Cysharp.Threading.Tasks;
using SanderSaveli.UDK;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class DataManager : MonoBehaviour
    {
        public IReadOnlyList<LevelSaveData> Levels => _levelDataManager.Levels;
        public LevelSaveData CurrentLevel;
        public int TotalLevels => Levels.Count;

        private LevelDataManager _levelDataManager;
        private bool _isLevelLoaded;

        void Awake()
        {
            _levelDataManager = new LevelDataManager(new JsonToFileStorageService());
            if (_levelDataManager.IsLoaded)
            {
                LevelsLoaded();
            }
            else
            {
                _levelDataManager.OnAllLevelDataLoaded += LevelsLoaded;
            }
        }

        public async UniTask<LevelData> GetFullLevelData(LevelSaveData levelSaveData)
        {
            await UniTask.WaitUntil(() => _isLevelLoaded);
            return _levelDataManager.GetFullLevelData(levelSaveData);
        }

        public void UpdateLevelSave(LevelSaveData levelSaveData)
        {
            _levelDataManager.UpdateLevelData(levelSaveData);
        }

        public LevelSaveData GetLevelByNumber(int number)
        {
            return Levels.FirstOrDefault(t=> t.level_number == number);
        }

        private void LevelsLoaded()
        {
            _levelDataManager.OnAllLevelDataLoaded -= LevelsLoaded;
            UpdateCurrentLevel();
            _isLevelLoaded = true;

        }

        private void UpdateCurrentLevel()
        {
            Debug.Log("Update curr level");
            LevelSaveData data = Levels.FirstOrDefault(t => !t.is_complete);
            if(data.Equals(default(LevelSaveData)))
            {
                CurrentLevel = Levels[Levels.Count - 1];
            }
            else
            {
                CurrentLevel = data;
            }
        }
    }
}
