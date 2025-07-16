using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public class SentryRifle : SentryTower
    {
        private const float AngleOffset = 90f;
        
        [SerializeField] private Transform bulletSpawnPoint;
        
        protected override void PerformAction()
        {
            Bullet bulletInstance = GameFactory.CreateBullet(at: bulletSpawnPoint, which: BulletPrefab);
            bulletInstance.Initialize(LockedTarget.position, Damage);
        }
    }
}