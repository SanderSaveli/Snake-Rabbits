using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class InputActionManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ISceneLoader _sceneLoader;

        [Inject]
        public void Construct(SignalBus signalBus, ISceneLoader sceneLoader)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalInputAction>(HandleInputAction);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalInputAction>(HandleInputAction);
        }

        private void HandleInputAction(SignalInputAction ctx)
        {
            switch (ctx.ActionType)
            {
                case InputActionType.LoadMenu:
                    _sceneLoader.LoadScene(SceneType.MenuScene);
                    break;
                case InputActionType.LoadGame_Arcade:
                    _sceneLoader.LoadScene(SceneType.GameScene);
                    break;
                case InputActionType.LoadGame_Levels:
                    _sceneLoader.LoadScene(SceneType.GameScene);
                    break;
                case InputActionType.ExitGame:
                    ExitGame();
                    break;
                case InputActionType.RestartScene:
                    _sceneLoader.RestartScene();
                    break;
                default:
                    throw new System.NotImplementedException("There is no case for ActionType: " + ctx.ActionType);
            }
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}
