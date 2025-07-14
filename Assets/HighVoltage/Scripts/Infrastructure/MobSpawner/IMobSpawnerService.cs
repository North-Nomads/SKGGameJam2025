using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Mobs;
using UnityEngine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public interface IMobSpawnerService : IService
    {
        event EventHandler<int> AnotherMobDied;
        List<MobBrain> CurrentlyAliveMobs { get; }
        void LoadConfigToSpawners(LevelConfig levelConfig, WaypointHolder[] spawnerSpots);
        void HandleMobReachedCore(MobBrain mob);
    }
}