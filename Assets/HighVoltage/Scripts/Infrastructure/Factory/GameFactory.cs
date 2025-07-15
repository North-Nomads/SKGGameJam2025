using UnityEngine;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Services.Progress;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.AssetManagement;

namespace HighVoltage.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPlayerProgressService _progress;
        private readonly MobBrain[] _mobPrototypes;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<IProgressUpdater> ProgressWriters { get; } = new()
        {
            Capacity = 0
        };

        public GameFactory(IAssetProvider assets, IPlayerProgressService progress)
        {
            _assets = assets;
            _progress = progress;
            _mobPrototypes = Resources.LoadAll<MobBrain>(AssetPath.MobPath);
        }

        public PlayerCore CreatePlayerCore(GameObject at) 
            => _assets.Instantiate<PlayerCore>(AssetPath.PlayerCorePrefabPath, at.transform.position);

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public Bullet CreateBullet(Transform at) 
            => _assets.Instantiate<Bullet>(AssetPath.BulletPrefab, at.position);

        public SentryTower CreateSentry(GameObject spawnPosition) 
            => _assets.Instantiate<SentryTower>(AssetPath.SentryPrefab, spawnPosition.transform.position);

        public MobBrain CreateMobOn(MobBrain whichEnemyPrefab, Vector3 at) 
            => Object.Instantiate(whichEnemyPrefab, at, Quaternion.identity);

        public PlayerBuildBehaviour CreateBuilder()
        {
            return _assets.Instantiate<PlayerBuildBehaviour>(AssetPath.BuilderPrefabPath, Vector3.zero);
            
        }
    }
}
