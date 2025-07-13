using System;
using UnityEngine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public interface IMobSpawnerService : IService
    {
        event EventHandler<int> AnotherMobDied;
        void LoadConfigToSpawners(LevelConfig levelConfig, GameObject[] spawnerSpots, GameObject playerCore);
    }
}