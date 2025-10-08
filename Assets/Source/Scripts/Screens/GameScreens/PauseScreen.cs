using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SanderSaveli.Snake
{
    public class PauseScreen : TimeStopScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _resume;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exitToMenu;

        protected override void SubscribeToEvents()
        {
            _resume.onClick.AddListener(HandleResume);
            _restart.onClick.AddListener(HandleRestart);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _resume.onClick.AddListener(HandleResume);
            _restart.onClick.AddListener(HandleRestart);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleResume()
        {
            
        }

        private void HandleRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HandleExitToMenu()
        {

        }
    }
}
