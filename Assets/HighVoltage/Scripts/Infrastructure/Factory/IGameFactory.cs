using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Sentry;
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
        PlayerBuildBehaviour CreateBuilder();
        Bullet CreateBullet(Transform at, Bullet which);
        SentryTower CreateSentry(Vector3Int spawnPosition, SentryConfig sentryConfig);
        PlayerCamera CreateCamera(Vector3 spawnPosition);
    }
}