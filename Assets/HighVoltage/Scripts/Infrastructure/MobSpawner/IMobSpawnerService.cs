using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public interface IMobSpawnerService : IService
    {
        event EventHandler AnotherMobDiedNoReward;
        event EventHandler<int> AnotherMobDied;
        List<MobBrain> CurrentlyAliveMobs { get; }
        bool IsWaveOngoing { get; }
        void LoadConfigToSpawners(MobWave waveConfig, WaypointHolder[] spawnerSpots, float deltaBetweenSpawns);
        void HandleMobReachedCore(MobBrain mob);
        void LaunchMobSpawning();
        void UpdateWaveContent(MobWave newWave);
        void UpdateWaveOngoingStatus(bool isWaveOngoing);
    }
}