using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ArcadeEndScreen : TimeStopScreen
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitToMenuButton;
        private IScoreManager _scoreManager;

        [Inject]
        public void Construct(IScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _exitToMenuButton.onClick.AddListener(HandleExit);
            _restartButton.onClick.AddListener(HandleRestart);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _exitToMenuButton.onClick.RemoveListener(HandleExit);
            _restartButton.onClick.RemoveListener(HandleRestart);
        }
        public override void Show(Action callback = null)
        {
            base.Show(callback);
            _scoreText.text = _scoreManager.Score.ToString();
        }

        private void HandleRestart()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Arcade));
        }

        private void HandleExit()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }
    }
}
