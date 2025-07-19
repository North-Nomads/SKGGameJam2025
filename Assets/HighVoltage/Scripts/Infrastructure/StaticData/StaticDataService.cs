using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HighVoltage.Enemy;
using HighVoltage.Infrastructure.Sentry;
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
        private Dictionary<int, SentryConfig> _sentryConfigs;
        private Dictionary<int, SwitchConfig> _switchConfigs;
        private Texture2D _tileAtlas;
        private LineRenderer _wirePrefab;

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
        public Texture2D GetTileAtlas() => _tileAtlas;

        public void LoadTileAtlas() => _tileAtlas = Resources.Load<Texture2D>("Textures/Tiles/TileAtlas");

        public SentryConfig ForSentryID(int sentryConfigID)
            => _sentryConfigs.GetValueOrDefault(sentryConfigID);
        
        public void LoadSentries()
            => _sentryConfigs = Resources.LoadAll<SentryConfig>("Configs/Sentries").ToDictionary(x => x.BuildingID, x => x);

        public MobConfig ForEnemyID(int zombieId)
            => _zombieConfigs.GetValueOrDefault(zombieId);
        
        public void LoadBuildingPrefabs() 
            => Resources.LoadAll<GameObject>("Prefabs/Buildings").ToList();
        
        public SwitchConfig ForSwitchID(int buildingID) 
            => _switchConfigs.GetValueOrDefault(buildingID);

        public void LoadBuildingConfigs() =>
            _switchConfigs = Resources.LoadAll<SwitchConfig>("Configs/Buildings")
                .ToDictionary(x => x.BuildingID, x => x);

        public LineRenderer GetWirePrefab() => _wirePrefab;

        public void LoadWirePrefab()
        {
            _wirePrefab = Resources.Load<LineRenderer>("Prefabs/Wire/Wire");
        }
    }
}