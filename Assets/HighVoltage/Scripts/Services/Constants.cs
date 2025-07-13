namespace HighVoltage.Services
{
    public static class Constants
    {
        public const string BootstrapSceneName = "Bootstrap";
        public const string HubSceneName = "Menu";
        public const string GameplayScene = "Main";
        public const string BossSceneName = "Boss";
        public const int GameplayScenesCount = 3;

        public const string WeaponSpawnPointTag = "WeaponSpawnPoint";
        public const string SurvivorSpawnPointTag = "SurvivorSpawnPoint";
        public const string PlayerSpawnPointTag = "PlayerSpawnPoint";
        public const string NextLevelPortalSpawnPoint = "NextLevelPortalSpawnPoint";

        public const float Epsilon = 0.00001f;
        public const float TimeToTick = .1f;
        public const int MaxWeaponUpgradeLevel = 5;
    }
}