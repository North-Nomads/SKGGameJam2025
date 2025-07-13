using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.HubVisuals
{
    public interface IHubVFX : IService
    {
        void PlayWeaponVFX(bool isNewWeapon);
        void ResetOnHubLoaded(IAssetProvider assetProvider);
    }
}