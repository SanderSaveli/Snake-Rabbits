using CustomText;
using SanderSaveli.UDK.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class LevelSlot : MonoBehaviour, ISlot<LevelSaveData>
    {
        public LevelSaveData LevelData { get; private set; }
        public Action<LevelSlot> OnSelected;

        [Header("Components")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private StarGroupView _stars;
        [SerializeField] private Button _button;
        [SerializeField] private ImageColorByType _backgroundColor;

        [Header("Params")]
        [SerializeField] private string _levelPrefix;
        [SerializeField] private Custom_ColorStyle _completeColor = Custom_ColorStyle.Green;
        [SerializeField] private Custom_ColorStyle _notUnlockColor = Custom_ColorStyle.Gray;
        [SerializeField] private Custom_ColorStyle _currentColor = Custom_ColorStyle.Blue;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickOnButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickOnButton);
        }

        public void Fill(LevelSaveData value)
        {
            LevelData = value;
            _backgroundColor.ChangeColor(value.is_complete ? _completeColor : _notUnlockColor);
            _button.interactable = value.is_complete;
            _stars.ShowStars(value.star_count);
            _levelText.text = _levelPrefix + value.level_number.ToString();
        }

        public void SetCurrent()
        {
            _button.interactable = true;
            _backgroundColor.ChangeColor(_currentColor);
        }

        private void ClickOnButton()
        {
            OnSelected?.Invoke(this);
        }
    }
}
