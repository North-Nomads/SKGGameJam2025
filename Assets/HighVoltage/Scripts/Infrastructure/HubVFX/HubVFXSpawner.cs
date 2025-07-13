using UnityEngine;
using HighVoltage.Services;

namespace HighVoltage.Infrastructure.HubVisuals
{
    public class HubVFXSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem weaponUpgradeVFX;
        [SerializeField] private ParticleSystem weaponPurchaseVFX;
        private Transform _weaponDisplayPoint;

        private void Awake()
        {
            _weaponDisplayPoint = GameObject.FindWithTag(Constants.WeaponSpawnPointTag).transform;
            weaponUpgradeVFX.transform.position = _weaponDisplayPoint.position;
            weaponPurchaseVFX.transform.position = _weaponDisplayPoint.position;
        }

        public void PlayWeaponUpgradeVFX() 
            => weaponUpgradeVFX.Play();

        public void PlayWeaponPurchaseVFX()
            => weaponPurchaseVFX.Play();
    }
}