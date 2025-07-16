using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public class SentrySniper : SentryTower
    {
        [SerializeField] private Transform bulletSpawnPoint;
        
        protected override void PerformAction()
        {
            Bullet bulletInstance = GameFactory.CreateBullet(at: bulletSpawnPoint, BulletPrefab);
            bulletInstance.Initialize(LockedTarget.position, Damage);
        }
    }
}