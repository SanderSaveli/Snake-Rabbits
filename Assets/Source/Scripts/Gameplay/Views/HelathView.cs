using DG.Tweening;
using SanderSaveli.UDK.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class HelathView : MonoBehaviour
    {
        [SerializeField] private ShowHideAnimation _showHideAnimation;
        [SerializeField] private Image _healthImage;
        [SerializeField] private TMP_Text _healthText;
        [Header("Properties")]
        [SerializeField] private float _showAnimationDuration = 0.5f;
        [SerializeField] private float _showDelay = 0.5f;
        [SerializeField] private float _showTime = 1f;
        [SerializeField] private int _blickTime = 8;

        private IHealthManager _healthManager;

        [Inject]
        public void Construct(IHealthManager healthManager)
        {
            _healthManager = healthManager;
        }

        private void Start()
        {
            if (_healthManager != null)
            {
                _healthManager.OnHealthChange += ShowHealthChange;
                ShowHealth(_healthManager.Health);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (_healthManager != null)
            {
                _healthManager.OnHealthChange -= ShowHealthChange;
            }
        }

        private void ShowHealthChange(int health)
        {
            _healthImage.DOFade(0, 0.3f).SetUpdate(false).SetLoops(8).SetLink(gameObject);
            ShowHealth(health);
        }

        private void ShowHealth(int health)
        {
            _healthText.text = health.ToString();
            _showHideAnimation.Show(_showDelay, _showAnimationDuration, 
                () => _showHideAnimation.Hide(_showTime, _showAnimationDuration, null));
        }
    }
}
