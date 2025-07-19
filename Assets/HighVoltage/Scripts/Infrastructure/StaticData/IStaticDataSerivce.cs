using HighVoltage.Enemy;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;
using UnityEngine;

namespace HighVoltage.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadLevels();
        LevelConfig ForLevel(int levelID);
        WindowConfig ForWindow(WindowId endGame);
        GameWindowConfig ForGameWindow(GameWindowId endGame);
        MobConfig ForEnemyID(int zombieId);
        void LoadEnemies();
        Texture2D GetTileAtlas();
        void LoadTileAtlas();
        SentryConfig ForSentryID(int sentryConfigID);
        void LoadSentries();
        void LoadBuildingPrefabs();
        SwitchConfig ForSwitchID(int buildingID);
        void LoadBuildingConfigs();
        LineRenderer GetWirePrefab();
        void LoadWirePrefab();
    }
}