using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameEndHandler : MonoBehaviour
    {
        [SerializeField] private WinScreen _screen;

        private SignalBus _signalBus;
        private IScoreManager _scoreManager;
        private DataManager _dataManager;
        private LevelConfig _levelConfig;

        [Inject]
        public void Construct(SignalBus signalBus, IScoreManager scoreManager, DataManager dataManager, LevelConfig levelConfig)
        {
            _signalBus = signalBus;
            _scoreManager = scoreManager;
            _dataManager = dataManager;
            _levelConfig = levelConfig;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private async void HandleGameEnd(SignalGameEnd ctx)
        {
            SignalDoPostGameAction doPostGameAction = new SignalDoPostGameAction(ctx.IsWin);
            _signalBus.Fire(doPostGameAction);
            List<UniTask> actions = new List<UniTask>(doPostGameAction.Subscribers);
            await HandleActions(actions);
            if (ctx.IsWin)
            {
                LevelSaveData levelSaveData = CreateDataForThisLevel();
                Debug.Log($"Level end data: \nStar Count: {levelSaveData.star_count} \n Score: {levelSaveData.max_score}");
                CheckForNewRecord(levelSaveData);
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Win));
                _screen.Init(levelSaveData);
            }
            else
            {
                _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Lose));
            }
        }

        private async UniTask HandleActions(List<UniTask> actions)
        {
            foreach (UniTask action in actions)
            {
                await action;
            }
        }

        private void CheckForNewRecord(LevelSaveData currData)
        {
            LevelSaveData bestData = _dataManager.GetLevelByNumber(_levelConfig.level_number);
            if(currData.max_score > bestData.max_score)
            {
                _dataManager.UpdateLevelSave(bestData);
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
