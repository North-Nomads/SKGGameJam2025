using System;
using UnityEngine;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.MobSpawnerService
{
    public interface IMobSpawnerService : IService
    {
        GameObject PlayerInstance { get; }

        event EventHandler<int> AnotherMobDied;

        void SpawnMobs(GameObject playerInstance);
    }
}