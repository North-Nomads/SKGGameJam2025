using UnityEngine;
using HighVoltage.Infrastructure.Factory;

namespace HighVoltage.Infrastructure.CameraService
{
    public class CinemachineCameraService : ICameraService
    {
        private readonly IGameFactory _gameFactory;
        private PlayerInput _playerInput;

        public CinemachineCameraService(IGameFactory gameFactory) 
            => _gameFactory = gameFactory;

        public PlayerCamera InitializeCamera(Vector3 spawnPosition) 
            => _gameFactory.CreateCamera(spawnPosition);
    }
}