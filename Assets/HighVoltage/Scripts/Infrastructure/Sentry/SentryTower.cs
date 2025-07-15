using System;
using System.Linq;
using System.Runtime.Serialization;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    public abstract class SentryTower : MonoBehaviour, ICurrentReciever
    {
        [SerializeField] private Transform rotatingPart;
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
        private ICurrentSource _currentProvider;
        private float _stunnedTimeLeft;

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

            if (_stunnedTimeLeft >= 0)
            {
                _stunnedTimeLeft -= Time.deltaTime;
                return;
            }
            
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
            
            _currentProvider.RequestPower(Config.PowerConsumption);
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

        public ICurrentSource CurrentProvider => _currentProvider;

        public void AttachToSource(ICurrentSource currentProvider)
        {
            _currentProvider = currentProvider;
            _currentProvider.OnOverload += HandleOverload;
        }

        private void HandleOverload(object sender, EventArgs e)
        {
            _stunnedTimeLeft = Config.StunTime;
        }
    }
}