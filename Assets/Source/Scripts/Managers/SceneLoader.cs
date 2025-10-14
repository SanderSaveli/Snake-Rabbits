using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SanderSaveli.Snake
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        private SceneType _currentScene;

        public void LoadScene(SceneType sceneType, Action callback = null)
        {
            _currentScene = sceneType;
            StartCoroutine(LoadSceneAsync(sceneType.ToString(), callback));
        }

        public void RestartScene(Action callback = null)
        {
            StartCoroutine(LoadSceneAsync(_currentScene.ToString(), callback));
        }

        private IEnumerator LoadSceneAsync(string sceneName, Action callback)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            callback?.Invoke();
        }
    }
}
