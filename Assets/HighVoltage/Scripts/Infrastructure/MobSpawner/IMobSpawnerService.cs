using System;
using HighVoltage.Infrastructure.Mobs;
using UnityEngine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public interface IMobSpawnerService : IService
    {
        event EventHandler<int> AnotherMobDied;
        void LoadConfigToSpawners(LevelConfig levelConfig, WaypointHolder[] spawnerSpots);
        void HandleMobReachedCore(MobBrain mob);
    }
}