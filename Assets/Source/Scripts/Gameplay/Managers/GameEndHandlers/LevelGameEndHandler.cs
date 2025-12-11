using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class LevelGameEndHandler : GameEndHandler
    {
        private DataManager _dataManager;
        private LevelConfig _levelConfig;
        private WinScreen _winScreen;
        private IScoreManager _scoreManager;

        [Inject]
        public void Construct(IScoreManager scoreManager, DataManager dataManager, LevelConfig levelConfig, WinScreen winScreen)
        {
            _scoreManager = scoreManager;
            _dataManager = dataManager;
            _levelConfig = levelConfig;
            _winScreen = winScreen;
        }

        public override void Lose()
        {
            _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Lose));
        }

        public override void Win()
        {
            LevelSaveData levelSaveData = CreateDataForThisLevel();
            Debug.Log($"Level end data: \nStar Count: {levelSaveData.star_count} \n Score: {levelSaveData.max_score}");
            CheckForNewRecord(levelSaveData);
            _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Win));
            _winScreen.Init(levelSaveData);
        }

        private void CheckForNewRecord(LevelSaveData currData)
        {
            LevelSaveData lastData = _dataManager.GetLevelByNumber(_levelConfig.level_number);
            if (currData.max_score > lastData.max_score)
            {
                _dataManager.UpdateLevelSave(currData);
            }
        }

        private LevelSaveData CreateDataForThisLevel()
        {
            int score = _scoreManager.Score;
            int starCount = 0;
            if (_levelConfig.score_for_third_star <= score)
            {
                starCount = 3;
            }
            else if (_levelConfig.score_for_second_star <= score)
            {
                starCount = 2;
            }
            else if (_levelConfig.score_for_first_star <= score)
            {
                starCount = 1;
            }
            return new LevelSaveData(_levelConfig.level_number, score, 0, starCount);
        }
    }
}
