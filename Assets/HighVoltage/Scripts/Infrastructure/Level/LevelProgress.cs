using System;
using System.Linq;
using HighVoltage.StaticData;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.MobSpawning;

namespace HighVoltage.Level
{
    public class LevelProgress : ILevelProgress
    {
        public event EventHandler WaveCleared = delegate { };
        public event EventHandler LevelCleared = delegate { };

        private readonly IMobSpawnerService _mobSpawner;
        private readonly IStaticDataService _staticData;
        private LevelConfig _loadedLevelConfig;
        private int _currentWaveIndex;
        private MobWave _loadedWave;

        public LevelConfig LoadedLevelConfig => _loadedLevelConfig;
        public MobWave LoadedWave => _loadedWave;


        public LevelProgress(IMobSpawnerService mobSpawner, IStaticDataService staticData)
        {
            _mobSpawner = mobSpawner;
            _mobSpawner.AnotherMobDied += HandleMobDeath;
            
            _staticData = staticData;
        }

        public void LoadLevelConfig(LevelConfig levelConfig)
        {
            _loadedLevelConfig = levelConfig;
            LoadCurrentWaveConfig();
        }

        public List<SentryConfig> GetSentriesForThisLevel()
            => _loadedLevelConfig.SentryIDs.Select(sentryID => _staticData.ForSentryID(sentryID)).ToList();

        private void LoadCurrentWaveConfig() 
            => _loadedWave = _loadedLevelConfig.MobWaves[_currentWaveIndex];

        private void HandleMobDeath(object sender, int mobsLeft)
        {
            if (mobsLeft != 0)
                return;

            if (_loadedLevelConfig.MobWaves.Length - 1 == _currentWaveIndex)
            {
                LevelCleared(this, EventArgs.Empty);
            }
            else
            {
                WaveCleared(this, EventArgs.Empty);
                _currentWaveIndex++;
                LoadCurrentWaveConfig();
            }
        }
    }
}