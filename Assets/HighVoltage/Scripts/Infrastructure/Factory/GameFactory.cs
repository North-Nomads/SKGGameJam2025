using UnityEngine;
using System.Collections.Generic;
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

        public GameObject CreatePlayerCore(GameObject at) 
            => _assets.Instantiate(AssetPath.PlayerCorePrefabPath, at.transform.position);

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        public MobBrain CreateMobOn(MobBrain whichEnemyPrefab, Vector3 at) 
            => Object.Instantiate(whichEnemyPrefab, at, Quaternion.identity);

        public PlayerBuildBehaviour CreateBuilder()
        {
            return _assets.Instantiate<PlayerBuildBehaviour>(AssetPath.BuilderPrefabPath, Vector3.zero);
            
        }
    }
}
