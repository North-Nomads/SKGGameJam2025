using Cinemachine;
using HighVoltage.Infrastructure.Services;
using UnityEngine;

namespace HighVoltage.Infrastructure.CameraService
{
    public interface ICameraService : IService
    {
        PlayerCamera InitializeCamera(Vector3 spawnPosition);
    }
}