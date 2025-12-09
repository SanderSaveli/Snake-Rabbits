namespace SanderSaveli.Snake
{
    public interface IGameModeManager
    {
        public GameMode GameMode { get; set; }
        public GameModeInstaller GetActualInstallerPrefab();
    }
}
