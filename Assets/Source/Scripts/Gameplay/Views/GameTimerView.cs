using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class GameTimerView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _progressImage;
        [SerializeField] private Image _timerImage;

        [Header("Properties")]
        [SerializeField] private float _progressToRing = 0.2f;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _ringColor = Color.red;
        [SerializeField] private float _rotateDegrees = 15f;
        [SerializeField] private float _oneTickTime = 0.2f;

        private bool _isRinging;
        private Sequence _ringTween;

        public void UpdateView(float amount)
        {
            _progressImage.fillAmount = amount;

            if (amount <= _progressToRing)
            {
                if (!_isRinging)
                    StartRinging();
            }
            else
            {
                if (_isRinging)
                    StopRing();
            }
        }

        private void StartRinging()
        {
            _isRinging = true;

            _ringTween?.Kill();
            _timerImage.color = _normalColor;

            float currentZ = _timerImage.transform.localEulerAngles.z;

            if (currentZ > 180f) currentZ -= 360f;

            _ringTween = DOTween.Sequence();

            _ringTween.Append(
                _timerImage.transform.DOLocalRotate(
                    new Vector3(0, 0, -_rotateDegrees),
                    _oneTickTime
                ).SetEase(Ease.InOutSine)
            );

            _ringTween.Append(
                _timerImage.transform.DOLocalRotate(
                    new Vector3(0, 0, _rotateDegrees),
                    _oneTickTime
                ).SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo)
            );

            _ringTween.Join(
                _timerImage.DOColor(_ringColor, _oneTickTime)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
            );
            _ringTween.SetLink(gameObject);
        }

        private void StopRing()
        {
            _isRinging = false;

            _ringTween?.Kill();
            _ringTween = null;

            _timerImage.transform.rotation = Quaternion.identity;
            _timerImage.color = _normalColor;
        }
    }
}
