using CustomText;
using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class LevelSlot : MonoBehaviour, ISlot<LevelData>
    {
        public LevelData LevelData {  get; private set; }
        public Action<LevelSlot> OnSelected; 

        [Header("Components")]
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private List<StarView> _stars;
        [SerializeField] private Button _button;
        [SerializeField] private ImageColorByType _backgroundColor;

        [Header("Params")]
        [SerializeField] private string _levelPrefix;
        [SerializeField] private Custom_ColorStyle _completeColor = Custom_ColorStyle.Green;
        [SerializeField] private Custom_ColorStyle _notUnlockColor = Custom_ColorStyle.Gray;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickOnButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickOnButton);
        }

        public void Fill(LevelData value)
        {
            LevelData = value;
            _backgroundColor.ChangeColor(value.IsPassed ? _completeColor : _notUnlockColor);
            int i = 1;

            foreach (var item in _stars)
            {
                item.SetEnable(i <= value.Stars);
                i++;
            }

            _levelText.text = _levelPrefix + value.LevelNumber.ToString();
        }

        private void ClickOnButton()
        {
            OnSelected?.Invoke(this);
        }
    }
}
