using HighVoltage.Enemy;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;

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
    }
}