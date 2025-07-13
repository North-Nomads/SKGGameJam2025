using Cinemachine;
using System;
using System.Linq;
using UnityEngine;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.Infrastructure.CameraService
{
    [Serializable]
    public class CameraKeyValue
    {
        public WindowId CameraPosition;
        public CinemachineFreeLook Camera;
    }

    public class CameraStorage : MonoBehaviour
    {
        [SerializeField] private CameraKeyValue[] _cameras;

        public CinemachineFreeLook GetCamera(WindowId cameraPosition)
        {
            CameraKeyValue cameraKeyValye = _cameras.FirstOrDefault(x => x.CameraPosition == cameraPosition);
            return cameraKeyValye?.Camera;
        }

        public CinemachineFreeLook GetDefaultCamera() 
            => _cameras[0].Camera;
    }
}