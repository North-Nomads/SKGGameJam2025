using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Services.Progress;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Services;

namespace HighVoltage.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPlayerProgressService _progress;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<IProgressUpdater> ProgressWriters { get; } = new()
        {
            Capacity = 0
        };

        public GameFactory(IAssetProvider assets, IPlayerProgressService progress)
        {
            _assets = assets;
            _progress = progress;
            Resources.LoadAll<MobBrain>(AssetPath.MobPath);
        }

        public PlayerCore CreatePlayerCore(GameObject at) 
            => _assets.Instantiate<PlayerCore>(AssetPath.PlayerCorePrefabPath, at.transform.position);

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public Bullet CreateBullet(Transform at, Bullet which) 
            => _assets.Instantiate(which, at.position);

        public SentryTower CreateSentry(Vector3Int spawnPosition, SentryConfig sentryConfig) 
            => _assets.Instantiate(sentryConfig.Prefab, spawnPosition);

        public SwitchMain CreateSwitch(Vector3Int spawnPosition, SwitchConfig thingToBuild)
            => _assets.Instantiate(thingToBuild.SwitchPrefab, spawnPosition);

        public PlayerCamera CreateCamera(Vector3 spawnPosition) 
            => _assets.Instantiate<PlayerCamera>(AssetPath.CameraPrefabPath, spawnPosition);

        public ICoroutineRunner CreateCoroutineRunner() 
            => _assets.Instantiate<MobCoroutineRunner>(AssetPath.MobCoroutineRunner, Vector3.zero);

        public MobBrain CreateMobOn(MobBrain whichEnemyPrefab, Vector3 at) 
            => Object.Instantiate(whichEnemyPrefab, at, Quaternion.identity);

        public PlayerBuildBehaviour CreateBuilder() 
            => _assets.Instantiate<PlayerBuildBehaviour>(AssetPath.BuilderPrefabPath, Vector3.zero);
    }
}
