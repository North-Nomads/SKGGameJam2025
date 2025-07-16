using System;
using System.Linq;
using HighVoltage.StaticData;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;

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
        private bool _isLastWave;

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

        public float GetCurrentWaveTimer()
            => _loadedWave.SecondsDelayBeforeWave;

        private void LoadCurrentWaveConfig() 
            => _loadedWave = _loadedLevelConfig.MobWaves[_currentWaveIndex];

        private void HandleMobDeath(object sender, int mobsLeft)
        {
            if (mobsLeft != 0)
                return;

            if (_isLastWave)
            {
                LevelCleared(this, EventArgs.Empty);
                Debug.Log("Level Cleared");
                return;
            }

            Debug.Log($"Wave {_currentWaveIndex + 1} cleared. New wave: {_currentWaveIndex + 2}/{_loadedLevelConfig.MobWaves.Length}");
            _currentWaveIndex++;
            _isLastWave = _currentWaveIndex == _loadedLevelConfig.MobWaves.Length - 1;
            LoadCurrentWaveConfig();
            WaveCleared(this, EventArgs.Empty);
        }
    }
}