using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ScoreView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _text;

        [Header("Params")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _aimationDuration = 0.5f;

        private IScoreManager _scoreManager;
        private SignalBus _signalBus;
        private Sequence _sequence;

        [Inject]
        public void Construct(IScoreManager scoreManager, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _scoreManager = scoreManager;
        }

        private void OnEnable()
        {
            _text.text = _scoreManager.Score.ToString();
            _signalBus.Subscribe<SignalScoreChanged>(HandleAddScore);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalScoreChanged>(HandleAddScore);
        }

        private void HandleAddScore(SignalScoreChanged ctx)
        {
            UpdateText(ctx.CurrentScore);
        }

        private void UpdateText(int score)
        {
            _text.text = score.ToString();
            _sequence?.Kill();

            _sequence = DOTween.Sequence(_text);
            _sequence
                .Append(transform.DOScale(_scaleFactor, _aimationDuration).SetEase(Ease.OutCirc))
                .Append(transform.DOScale(1, _aimationDuration)).SetEase(Ease.InSine)
                .OnKill(() => _sequence = null);

        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}
