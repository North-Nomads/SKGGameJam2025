using System;
using System.Linq;
using HighVoltage.StaticData;
using System.Collections.Generic;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;

namespace HighVoltage.Level
{
    public class LevelProgress : ILevelProgress
    {
        public event EventHandler WaveCleared = delegate { };
        public event EventHandler LevelCleared = delegate { };
        public event EventHandler PlayerCoreDestroyed = delegate { };

        private readonly IMobSpawnerService _mobSpawner;
        private readonly IStaticDataService _staticData;
        private readonly IBuildingStoreService _buildingStore;
        private LevelConfig _loadedLevelConfig;
        private PlayerCore _playerCore;
        
        private int _mobsLeftThisWave;
        private int _currentWaveIndex;
        private MobWave _loadedWave;
        private bool _isLastWave;

        public LevelConfig LoadedLevelConfig => _loadedLevelConfig;
        public MobWave LoadedWave => _loadedWave;

        public bool IsLevelSuccessfullyFinished { get; private set; }

        public LevelProgress(IMobSpawnerService mobSpawner, IStaticDataService staticData, IBuildingStoreService buildingStore)
        {
            _mobSpawner = mobSpawner;
            _mobSpawner.AnotherMobDied += HandleMobDeath;
            
            _staticData = staticData;
            _buildingStore = buildingStore;
        }

        public void LoadLevelConfig(LevelConfig levelConfig, PlayerCore playerCore)
        {
            IsLevelSuccessfullyFinished = false;
            _loadedLevelConfig = levelConfig;
            _playerCore = playerCore;
            _playerCore.OnCoreHealthChanged += CheckPlayerCoreWasDestroyed;
            LoadCurrentWaveConfig();
        }

        private void CheckPlayerCoreWasDestroyed(object sender, int healthRemaining)
        {
            if (healthRemaining > 0)
                return;
            PlayerCoreDestroyed(this, EventArgs.Empty);
        }

        public List<SentryConfig> GetSentriesForThisLevel()
            => _loadedLevelConfig.SentryIDs.Select(sentryID => _staticData.ForSentryID(sentryID)).ToList();

        public float GetCurrentWaveTimer()
            => _loadedWave.SecondsDelayBeforeWave;

        private void LoadCurrentWaveConfig()
        {
            _loadedWave = _loadedLevelConfig.MobWaves[_currentWaveIndex];
            _mobsLeftThisWave = _loadedWave.Gates.Sum(gate => gate.LevelEnemies.Sum(enemy => enemy.Quantity));
        }

        private void HandleMobDeath(object sender, int mobID)
        {
            _buildingStore.AddMoney(_staticData.ForSentryID(mobID).Reward);
            _mobsLeftThisWave--;
            
            if (_mobsLeftThisWave != 0)
                return;

            if (_isLastWave)
            {
                IsLevelSuccessfullyFinished = true;
                LevelCleared(this, EventArgs.Empty);
                return;
            }

            _currentWaveIndex++;
            _isLastWave = _currentWaveIndex == _loadedLevelConfig.MobWaves.Length - 1;
            LoadCurrentWaveConfig();
            WaveCleared(this, EventArgs.Empty);
        }
    }
}