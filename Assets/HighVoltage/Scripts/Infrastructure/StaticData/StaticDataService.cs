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
        private Dictionary<int, EnemyConfig> _zombieConfigs;
        private Dictionary<int, LevelConfig> _levels;
        private int _totalWeapon;
        private Dictionary<int, LevelTaskConfig> _levelTasks;
        private Texture2D _tileAtlas;

        public int TotalWeapons => _totalWeapon;

        public void LoadLevels()
            => _levels = Resources.LoadAll<LevelConfig>("Levels").ToDictionary(x => x.LevelID, x => x);

        public LevelConfig ForLevel(int levelID)
            => _levels.TryGetValue(levelID, out LevelConfig levelConfig) ? levelConfig : null;

        public void LoadWindows()
            => _windowConfigs = Resources.Load<WindowStaticData>("UI/Windows").Configs.ToDictionary(x => x.WindowId, x => x);

        public WindowConfig ForWindow(WindowId windowID)
            => _windowConfigs.TryGetValue(windowID, out WindowConfig windowConfig) ? windowConfig : null;

        public void LoadGameWindows()
            => _gameWindowConfigs = Resources.Load<GameWindowStaticData>("UI/GameWindows").Configs.ToDictionary(x => x.WindowId, x => x);

        public GameWindowConfig ForGameWindow(GameWindowId windowId)
            => _gameWindowConfigs.TryGetValue(windowId, out GameWindowConfig windowConfig) ? windowConfig : null;

        public void LoadEnemies()
            => _zombieConfigs = Resources.LoadAll<EnemyConfig>("Zombies/").ToDictionary(x => x.EnemyId, x => x);

        public EnemyConfig ForEnemyID(int zombieId)
            => _zombieConfigs.TryGetValue(zombieId, out EnemyConfig config) ? config : null;

        public void LoadLevelTasks()
            => _levelTasks = Resources.LoadAll<LevelTaskConfig>("Tasks/").ToDictionary(x => x.TaskID, x => x);

        public LevelTaskConfig GetRandomLevelTask()
            => _levelTasks.ElementAt(Random.Range(0, _levelTasks.Count)).Value;

        public Texture2D GetTileAtlas() => _tileAtlas;

        public void LoadTileAtlas() => _tileAtlas = Resources.Load<Texture2D>("Textures/Tiles/TileAtlas.png");
    }
}