using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.Snake
{
    public class GameUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _pause;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _pause.onClick.AddListener(HandleOpenPause);
        }

        private void OnDisable()
        {
            _pause.onClick.RemoveListener(HandleOpenPause);
        }

        private void HandleOpenPause()
        {
            _signalBus.Fire(new SignalInputOpenGameScreen(GameScreenType.Pause));
        }
    }
}
