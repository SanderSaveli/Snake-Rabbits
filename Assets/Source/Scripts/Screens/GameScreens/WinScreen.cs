using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class WinScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _exitToMenu;

        protected override void SubscribeToEvents()
        {
            _nextLevel.onClick.AddListener(HandleNextLevel);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _nextLevel.onClick.RemoveListener(HandleNextLevel);
            _exitToMenu.onClick.RemoveListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleExitToMenu()
        {

        }

        private void HandleNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
