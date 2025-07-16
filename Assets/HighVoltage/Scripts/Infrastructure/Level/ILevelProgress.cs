using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Level
{
    public interface ILevelProgress : IService
    {
        event EventHandler WaveCleared;
        event EventHandler LevelCleared;
        event EventHandler PlayerCoreDestroyed;
        LevelConfig LoadedLevelConfig { get; }
        MobWave LoadedWave { get; }
        bool IsLevelSuccessfullyFinished { get; }
        void LoadLevelConfig(LevelConfig levelConfig, PlayerCore playerCore);
        List<SentryConfig> GetSentriesForThisLevel();
        float GetCurrentWaveTimer();
    }
}