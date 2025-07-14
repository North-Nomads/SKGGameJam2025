using UnityEngine;
using System.Collections.Generic;
using HighVoltage.HighVoltage.Scripts.Sentry;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        PlayerCore CreatePlayerCore(GameObject at);
        List<ISavedProgressReader> ProgressReaders { get; } 
        List<IProgressUpdater> ProgressWriters { get; }
        MobBrain CreateMobOn(MobBrain whichEnemyPrefab, Vector3 point);
        void CleanUp();
        Bullet CreateBullet(Transform at);
        SentryTower CreateSentry(GameObject spawnPosition);
    }
}