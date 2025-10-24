using DG.Tweening;
using SanderSaveli.UDK;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ScoreEffect : MonoBehaviour, IPoolableObject<ScoreEffect>
    {
        public Action<ScoreEffect> OnBackToPool { get; set; }

        [Header("Components")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Params")]
        [SerializeField] private float _height;
        [SerializeField] private float _upTime;
        [SerializeField] private float _delayTime;
        [SerializeField] private float _toTargetTime;

        private ScoreTarget _target;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(ScoreTarget scoreTarget, SignalBus signalBus)
        {
            _target = scoreTarget;
            _signalBus = signalBus;
        }

        public void OnActive()
        {
            _canvasGroup.alpha = 0f;
        }

        public void ShowScore(int Score, Vector3 position)
        {
            transform.position = position;
            _scoreText.text = Score.ToString();
            _canvasGroup.alpha = 0f;
            Vector3 upPos = new Vector3(0, _height, 0);
            Sequence sequence = DOTween.Sequence(transform);

            sequence
                .Append(transform.DOMove(position + upPos, _upTime))
                .Join(_canvasGroup.DOFade(1, _upTime))
                .AppendInterval(_delayTime)
                .Append(transform.DOMove(_target.transform.position, _toTargetTime))
                .JoinCallback(() => _signalBus.Fire(new SignalAddScore(Score)))
                .Join(_canvasGroup.DOFade(0, _toTargetTime))
                .OnComplete(() => {
                    OnBackToPool.Invoke(this);
                    })
                .SetLink(gameObject);
        }
    }
}
