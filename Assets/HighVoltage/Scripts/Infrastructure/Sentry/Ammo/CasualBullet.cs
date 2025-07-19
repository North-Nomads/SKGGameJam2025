using HighVoltage.Infrastructure.Mobs;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public class CasualBullet : Bullet
    {
        protected override void HandleContact(Collider2D mob)
        {
            mob.GetComponent<MobBrain>().TakeDamage(BulletDamage);
            Destroy(gameObject);
        }
    }
}