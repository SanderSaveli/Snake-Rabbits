using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class WinScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _exitToMenu;

        [Header("Components")]
        [SerializeField] private StarGroupView _starRowView;
        [SerializeField] private TMP_Text _scoreText;

        private SignalBus _signalBus;
        private DataManager _dataManager;
        private LevelConfigTransitor _levelConfigTransitor;

        [Inject]
        public void Construct(SignalBus signalBus, DataManager dataManager, LevelConfigTransitor levelConfigTransitor)
        {
            _signalBus = signalBus;
            _dataManager = dataManager;
            _levelConfigTransitor = levelConfigTransitor;
        }

        public void Init(LevelSaveData currData)
        {
            _scoreText.text = currData.max_score.ToString();
            _starRowView.ShowStars(currData.star_count);
        }

        protected override void SubscribeToEvents()
        {
            _nextLevel.onClick.AddListener(HandleNextLevel);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _nextLevel.onClick.RemoveListener(HandleNextLevel);
            _exitToMenu.onClick.RemoveListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleExitToMenu()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }

        private void HandleNextLevel()
        {
            int nextLevel = Mathf.Min(_levelConfigTransitor.Config.level_number + 1, _dataManager.TotalLevels);
            _levelConfigTransitor.SetLevel(nextLevel);
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Levels));
        }
    }
}
