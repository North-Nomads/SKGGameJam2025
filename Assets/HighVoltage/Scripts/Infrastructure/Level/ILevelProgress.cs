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
        LevelConfig LoadedLevelConfig { get; }
        MobWave LoadedWave { get; }
        void LoadLevelConfig(LevelConfig levelConfig);
        List<SentryConfig> GetSentriesForThisLevel();
    }
}