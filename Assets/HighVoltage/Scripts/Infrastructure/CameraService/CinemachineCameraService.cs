using Cinemachine;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.Infrastructure.CameraService
{
    public class CinemachineCameraService : ICameraService
    {
        private readonly IGameFactory _gameFactory;
        private CameraStorage _cameraStorage;
        
        private CinemachineFreeLook _currentCamera;

        public CinemachineCameraService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Cleanup()
        {
            _currentCamera = null;
            _cameraStorage = null;
        }

        public void MoveCamera(WindowId newCameraPosition)
        {
            
        }
    }
}