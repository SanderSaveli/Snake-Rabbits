using SanderSaveli.Snake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.Pathfinding
{
    public class PathFinderInstaller : MonoInstaller
    {
        [SerializeField] private GraficConfig _graficConfig;

        public override void InstallBindings()
        {
            Container.Bind<GraficConfig>().FromInstance(_graficConfig).AsSingle().NonLazy();
        }
    }
}
