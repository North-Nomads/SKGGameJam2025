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
        int TotalWeapons { get; }
        void LoadLevels();
        LevelConfig ForLevel(int levelID);
        WindowConfig ForWindow(WindowId endGame);
        GameWindowConfig ForGameWindow(GameWindowId endGame);
        void LoadEnemies();
        EnemyConfig ForEnemyID(int zombieId);
        void LoadLevelTasks();
        LevelTaskConfig GetRandomLevelTask();
        Texture2D GetTileAtlas();
        void LoadTileAtlas();
    }
}