using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class ScoreForAppleGiver : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalAppleEated>(HandleEatApple);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalAppleEated>(HandleEatApple);
        }

        private void HandleEatApple(SignalAppleEated ctx)
        {
            _signalBus.Fire(new SignalAddScore(1));
        }
    }
}
