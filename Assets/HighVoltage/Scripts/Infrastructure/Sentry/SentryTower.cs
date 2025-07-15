using System.Linq;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public abstract class SentryTower : MonoBehaviour
    {
        [SerializeField] private Transform rotatingPart;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField, Min(0)] private float scanRadius;
        
        private const float OneSecond = 1;

        protected Bullet BulletPrefab => bulletPrefab;
        
        protected Transform LockedTarget;
        protected IMobSpawnerService MobSpawnerService;
        protected SentryConfig Config;
        protected IGameFactory GameFactory;
        protected bool IsActionOnCooldown => ActionCooldownTimeLeft > 0f;
        protected float ActionCooldownTimeLeft;
        protected float MaxCooldownTime;
        protected int MaxDurability;
        protected int CurrentDurability;
        protected int Damage;
        protected int DecayPerSecond;

        
        private float _decayCooldownTimeLeft;

        public void Initialize(SentryConfig config, IMobSpawnerService mobSpawnerService, IGameFactory gameFactory)
        {
            Config = config;
            MobSpawnerService = mobSpawnerService;
            GameFactory = gameFactory;
            
            MaxDurability = config.MaxDurability;
            CurrentDurability = MaxDurability;
            DecayPerSecond = config.DecayPerSecond;
            _decayCooldownTimeLeft = OneSecond; 
            
            Damage = Config.Damage;
            
            MaxCooldownTime = config.TimeBetweenActions;
            ActionCooldownTimeLeft = 0f;
        }

        protected abstract void PerformAction();

        protected virtual void Update()
        {
            KeepDecay();
            ScanForTarget();
            KeepTrackingEnemy();
            if (IsActionOnCooldown)
            {
                ActionCooldownTimeLeft -= Time.deltaTime;
                return;
            }

            if (LockedTarget == null)
                return;
            
            PerformAction();
            ActionCooldownTimeLeft = MaxCooldownTime;
        }

        protected virtual void ScanForTarget()
        {
            if (LockedTarget != null && Vector3.Distance(LockedTarget.transform.position, transform.position) <= scanRadius)
                return;


            LockedTarget = MobSpawnerService.CurrentlyAliveMobs
                .Where(x => Vector3.Distance(transform.position, x.transform.position) <= scanRadius)
                .OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude)
                .FirstOrDefault()?
                .transform;
        }

        protected virtual void KeepDecay()
        {
            if (_decayCooldownTimeLeft > 0f)
            {
                _decayCooldownTimeLeft -= Time.deltaTime;
                return;
            }
            
            _decayCooldownTimeLeft = OneSecond;
            CurrentDurability -= DecayPerSecond;
            if (CurrentDurability <= 0)
                DestroyBuilding();
        }

        protected virtual void DestroyBuilding()
        {
            Destroy(gameObject);
        }
        
        protected virtual void KeepTrackingEnemy()
        {
            if (LockedTarget == null)
                return;
            
            Vector3 direction = LockedTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float AngleOffset = 90f;
            rotatingPart.rotation = Quaternion.Euler(0, 0, angle + AngleOffset);
        }
    }
}