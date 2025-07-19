using HighVoltage.Infrastructure.Mobs;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public class ExplosiveBullet : Bullet
    {
        [SerializeField] private float explosionRadius;
        
        protected override void HandleContact(Collider2D mob)
        {
            Collider2D[] explosionHitMobs = Physics2D.OverlapCircleAll(mob.transform.position, explosionRadius);
            foreach (Collider2D hitMob in explosionHitMobs) 
                hitMob.GetComponent<MobBrain>().TakeDamage(BulletDamage);
            Destroy(gameObject);
        }
    }
}