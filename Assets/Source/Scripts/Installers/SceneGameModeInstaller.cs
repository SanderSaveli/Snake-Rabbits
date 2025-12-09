using UnityEngine;
using Zenject;

namespace SanderSaveli.Snake
{
    public class SceneGameModeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            IGameModeManager modeManager = Container.Resolve<IGameModeManager>();
            GameModeInstaller modeInstaller = modeManager.GetActualInstallerPrefab();

            modeInstaller.gameObject.SetActive(false);
            GameModeInstaller modeInstance = Instantiate(modeInstaller);
            modeInstance.SetContainer(Container);
            modeInstance.InstallBindings();
            foreach (var component in modeInstance.GetComponents<MonoBehaviour>())
            {
                Container.Inject(component);
            }
            modeInstance.gameObject.SetActive(true);
            modeInstaller.gameObject.SetActive(true);
        }
    }
}
