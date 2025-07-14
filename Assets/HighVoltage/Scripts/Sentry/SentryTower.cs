using System.Linq;
using System.Runtime.Serialization;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;

namespace HighVoltage.HighVoltage.Scripts.Sentry
{
    public abstract class SentryTower : MonoBehaviour
    {
        private const float OneSecond = 1;
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
        protected abstract void KeepTrackingEnemy();

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
            if (LockedTarget != null)
                return;

            LockedTarget = MobSpawnerService.CurrentlyAliveMobs
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
            Debug.Log("Building destroyed");
        }
    }
}