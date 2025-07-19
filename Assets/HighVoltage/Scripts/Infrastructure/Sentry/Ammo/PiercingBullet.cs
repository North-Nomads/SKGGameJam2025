using HighVoltage.Infrastructure.Mobs;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public class PiercingBullet : Bullet
    {
        [SerializeField] private int maxPiercedEnemies;
        private int _enemiesToPierceLeft;
        
        protected override void HandleContact(Collider2D mob)
        {
            if (_enemiesToPierceLeft > 0)
            {
                mob.GetComponent<MobBrain>().TakeDamage(BulletDamage);
                _enemiesToPierceLeft--;
                return;
            }
                
            Destroy(gameObject);
        }
    }
}