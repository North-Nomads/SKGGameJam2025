using HighVoltage.Infrastructure.AssetManagement;

namespace HighVoltage.Infrastructure.HubVisuals
{
    public class HubVFX : IHubVFX
    {
        private HubVFXSpawner _vfxSpawner;

        public void ResetOnHubLoaded(IAssetProvider assetProvider) 
            => _vfxSpawner = assetProvider.Instantiate<HubVFXSpawner>(AssetPath.HubVFXSpawner);

        public void PlayWeaponVFX(bool isNewWeapon)
        {
            if (isNewWeapon) 
                _vfxSpawner.PlayWeaponPurchaseVFX();
            else
                _vfxSpawner.PlayWeaponUpgradeVFX();
        }
    }
}