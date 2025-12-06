using Cysharp.Threading.Tasks;
using SanderSaveli.UDK.UI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private LevelFiller _levelFiller;
        [SerializeField] private LevelInfoPopup _levelPopup;

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
                _levelFiller.FillItems(_dataManager.Levels.ToList());
                _isShow = true;
            }
        }

        public override async void ShowImmediately()
        {
            base.ShowImmediately();
            if (!_isShow)
            {
                await UniTask.WaitUntil(() => _dataManager.IsLevelLoaded);
                _levelFiller.FillItems(_dataManager.Levels.ToList());
                foreach(var slot in _levelFiller.Slots)
                {
                    slot.OnSelected += HandleLevelSelect;
                }

                _isShow = true;
                OpenCurrentLevel();
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
            foreach (var slot in _levelFiller.Slots)
            {
                slot.OnSelected -= HandleLevelSelect;
            }
            base.UnsubscribeFromEvents();
        }

        private async void HandlePlay()
        {
            LevelData level = await _dataManager.GetFullLevelData(_dataManager.CurrentLevel);
            _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.LevelInfo));
            _levelPopup.Init(level);
        }

        private void HandleExit()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.ExitGame));
        }

        private void OpenCurrentLevel()
        {
            LevelSlot currentSlot = _levelFiller.Slots.FirstOrDefault(t => t.LevelData.level_number == _dataManager.CurrentLevel.level_number);

            currentSlot.SetCurrent();
        }

        private async void HandleLevelSelect(LevelSlot levelSlot)
        {
            LevelData level = await _dataManager.GetFullLevelData(levelSlot.LevelData);
            _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.LevelInfo));
            _levelPopup.Init(level);
        }
    }
}
