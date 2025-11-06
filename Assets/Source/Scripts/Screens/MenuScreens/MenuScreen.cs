using SanderSaveli.UDK.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private LevelFiller _levelFiller;

        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        private SignalBus _signalBus;
        private DataManager _dataManager;
        private bool _isShow;

        [Inject]
        public void Construct(SignalBus signalBus, DataManager dataManager)
        {
            _signalBus = signalBus;
            _dataManager = dataManager;
        }

        public override void Show(Action callback = null)
        {
            base.Show(callback);
            if(!_isShow)
            {
                Debug.Log("Level count : " + _dataManager.Levels.Count);
                _levelFiller.FillItems(_dataManager.Levels);
                _isShow = true;
            }
        }

        public override void ShowImmediately()
        {
            base.ShowImmediately();
            if (!_isShow)
            {
                _levelFiller.FillItems(_dataManager.Levels);
                _isShow = true;
            }
        }

        protected override void SubscribeToEvents()
        {
            _playButton.onClick.AddListener(HandlePlay);
            _exitButton.onClick.AddListener(HandleExit);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _playButton.onClick.RemoveListener(HandlePlay);
            _exitButton.onClick.RemoveListener(HandleExit);
            base.UnsubscribeFromEvents();
        }

        private void HandlePlay()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame_Levels));
        }

        private void HandleExit()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.ExitGame));
        }
    }
}
