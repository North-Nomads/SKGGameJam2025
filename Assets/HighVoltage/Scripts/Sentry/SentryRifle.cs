using UnityEngine;

namespace HighVoltage.HighVoltage.Scripts.Sentry
{
    public class SentryRifle : SentryTower
    {
        private const float AngleOffset = 90f;
        
        [SerializeField] private Transform rotatingPart;
        [SerializeField] private Transform bulletSpawnPoint;
        
        protected override void PerformAction()
        {
            Bullet bulletInstance = GameFactory.CreateBullet(at: bulletSpawnPoint);
            bulletInstance.Initialize(LockedTarget.position, Damage);
        }

        protected override void KeepTrackingEnemy()
        {
            if (LockedTarget == null)
                return;
            
            Vector3 direction = LockedTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotatingPart.rotation = Quaternion.Euler(0, 0, angle + AngleOffset);
            
        }

        
    }
}