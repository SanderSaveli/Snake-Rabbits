using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class LoseScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exitToMenu;

        protected override void SubscribeToEvents()
        {
            _restart.onClick.AddListener(HandleNextLevel);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _restart.onClick.RemoveListener(HandleNextLevel);
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
