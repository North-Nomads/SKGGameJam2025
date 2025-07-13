using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Interactables;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.ModelDisplayService;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        List<ISavedProgressReader> ProgressReaders { get; } 
        List<IProgressUpdater> ProgressWriters { get; }
        CameraStorage InstantiateCameraStorage();
        HubModelDisplayer CreateModelDisplayer();
        void CleanUp();
        CinemachineVirtualCamera CreateCamera(GameObject playerInstance);
        MobBrain CreateMobOn(GameObject point);
        GameObject CreatePullSpell(Vector3 spawnPosition);
        NextLevelPortal CreateNextLevelPortal(GameObject at);
    }
}