using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class LevelInfoPopup : Popup
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _levelNumberText;
        [SerializeField] private TMP_Text _bestScore;
        [SerializeField] private Button _playButton;
        [SerializeField] private List<StarView> _stars;
        [SerializeField] private List<TMP_Text> _starsScore;

        [Header("Parrams")]
        [SerializeField] private string _levelPrefix;
        private LevelData _levelData;
        private LevelConfigTransitor _levelConfigTransitor;

        [Inject]
        public void Construct(LevelConfigTransitor levelConfigTransitor)
        {
            _levelConfigTransitor = levelConfigTransitor;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _playButton.onClick.AddListener(HandlePlay);
        }
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _playButton.onClick.RemoveListener(HandlePlay);
        }

        public void Init(LevelData levelData)
        {
            _levelData = levelData;
            _levelNumberText.text = _levelPrefix + levelData.LevelNumber;
            _bestScore.text = levelData.MaxScore.ToString();
            int i = 1;
            foreach (var star in _stars)
            {
                star.SetEnable(i <= levelData.Stars);
                i++;
            }
            _starsScore[0].text = levelData.ScoreForFirstStar.ToString();
            _starsScore[1].text = levelData.ScoreForSecondStar.ToString();
            _starsScore[2].text = levelData.ScoreForThirdStar.ToString();
        }

        private void HandlePlay()
        {
            _levelConfigTransitor.SetLevel(_levelData.LevelNumber);
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Levels));
        }
    }
}
