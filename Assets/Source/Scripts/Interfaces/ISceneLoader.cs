using System;

namespace SanderSaveli.Snake
{
    public interface ISceneLoader
    {
        public void LoadScene(SceneType sceneType, Action callback = null);

        public void RestartScene(Action callback = null);
    }
}
