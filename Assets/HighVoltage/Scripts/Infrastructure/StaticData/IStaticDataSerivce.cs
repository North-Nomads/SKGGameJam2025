using HighVoltage.Enemy;
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
        void LoadEnemies();
        MobConfig ForEnemyID(int zombieId);
        Texture2D GetTileAtlas();
        void LoadTileAtlas();
        void LoadBuildingPrefabs();
        GameObject GetBuildingPrefab(int prefabID);
    }
}