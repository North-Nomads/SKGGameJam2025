using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Interactables;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.ModelDisplayService;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPlayerProgressService _progress;
        private readonly MobBrain[] _mobPrototypes;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<IProgressUpdater> ProgressWriters { get; } = new List<IProgressUpdater> { };

        public GameFactory(IAssetProvider assets, IPlayerProgressService progress)
        {
            _assets = assets;
            _progress = progress;
            _mobPrototypes = Resources.LoadAll<MobBrain>(AssetPath.MobPath);
        }

        public GameObject CreateHero(GameObject at) 
            => _assets.Instantiate(AssetPath.PlayerPrefabPath, at.transform.position);

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public CameraStorage InstantiateCameraStorage() 
            => _assets.Instantiate<CameraStorage>(AssetPath.HubCameraStorage);

        public HubModelDisplayer CreateModelDisplayer()
            => _assets.Instantiate<HubModelDisplayer>(AssetPath.HubModelDisplayer);

        public CinemachineVirtualCamera CreateCamera(GameObject playerInstance)
            => _assets.Instantiate<CinemachineVirtualCamera>(AssetPath.CameraPrefab);

        public MobBrain CreateMobOn(GameObject at) 
            => Object.Instantiate(_mobPrototypes[Random.Range(0, _mobPrototypes.Length)], at.transform.position, Quaternion.identity);

        public GameObject CreatePullSpell(Vector3 spawnPosition)
            => _assets.Instantiate(AssetPath.PullSpellPrefab, spawnPosition);

        public NextLevelPortal CreateNextLevelPortal(GameObject at)
            => _assets.Instantiate<NextLevelPortal>(AssetPath.NextLevelPortalPrefab, at.transform.position);
    }
}
