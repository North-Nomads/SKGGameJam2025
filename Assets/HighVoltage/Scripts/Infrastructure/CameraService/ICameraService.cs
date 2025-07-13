using HighVoltage.Infrastructure.Services;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.Infrastructure.CameraService
{
    public interface ICameraService : IService
    {
        public void MoveCamera(WindowId newCameraPosition);
        public void Cleanup();
    }
}