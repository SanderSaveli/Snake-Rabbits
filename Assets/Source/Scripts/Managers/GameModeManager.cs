using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class GameModeManager : MonoBehaviour, IGameModeManager
    {
        [Serializable]
        public class GameModePair
        {
            public GameMode GameMode;
            public GameModeInstaller Installer;
        }

        public GameMode GameMode { get; set; }
        [SerializeField] private List<GameModePair> _gameModes = new List<GameModePair>();

        public GameModeInstaller GetActualInstallerPrefab() =>
            _gameModes.FirstOrDefault(t => t.GameMode == GameMode).Installer;

    }
}
