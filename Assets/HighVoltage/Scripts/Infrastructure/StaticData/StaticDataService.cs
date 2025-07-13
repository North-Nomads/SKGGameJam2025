using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HighVoltage.Enemy;
using HighVoltage.Level;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<GameWindowId, GameWindowConfig> _gameWindowConfigs;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private Dictionary<int, MobConfig> _zombieConfigs;
        private Dictionary<int, LevelConfig> _levels;
        private int _totalWeapon;
        private Dictionary<int, LevelTaskConfig> _levelTasks;

        public void LoadLevels()
            => _levels = Resources.LoadAll<LevelConfig>("Configs/Levels").ToDictionary(x => x.LevelID, x => x);

        public LevelConfig ForLevel(int levelID)
            => _levels.GetValueOrDefault(levelID);

        public void LoadWindows()
            => _windowConfigs = Resources.Load<WindowStaticData>("UI/Windows").Configs.ToDictionary(x => x.WindowId, x => x);

        public WindowConfig ForWindow(WindowId windowID)
            => _windowConfigs.GetValueOrDefault(windowID);

        public void LoadGameWindows()
            => _gameWindowConfigs = Resources.Load<GameWindowStaticData>("UI/GameWindows").Configs.ToDictionary(x => x.WindowId, x => x);

        public GameWindowConfig ForGameWindow(GameWindowId windowId)
            => _gameWindowConfigs.GetValueOrDefault(windowId);

        public void LoadEnemies()
            => _zombieConfigs = Resources.LoadAll<MobConfig>("Configs/Mobs").ToDictionary(x => x.EnemyId, x => x);

        public MobConfig ForEnemyID(int zombieId)
            => _zombieConfigs.GetValueOrDefault(zombieId);
    }
}