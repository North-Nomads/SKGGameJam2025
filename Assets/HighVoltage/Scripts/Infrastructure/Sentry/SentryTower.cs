using System;
using System.IO;
using System.Linq;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.Infrastructure.Sentry
{
    public abstract class SentryTower : MonoBehaviour, ICurrentReceiver, IHealthOwner
    {
        [SerializeField] private Transform rotatingPart;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField, Min(0)] private float scanRadius;
        public event EventHandler<float> NotifyHealthBar = delegate { };
        public int MaxHealth { get; private set; }

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                NotifyHealthBar(this, (float)_currentHealth / MaxHealth);
                if (_currentHealth <= 0)
                    DestroyBuilding();
            }
        }

        protected Bullet BulletPrefab => bulletPrefab;
        
        protected Transform LockedTarget;
        protected IMobSpawnerService MobSpawnerService;
        protected SentryConfig Config;
        protected IGameFactory GameFactory;
        protected bool IsActionOnCooldown => ActionCooldownTimeLeft > 0f;
        protected float ActionCooldownTimeLeft;
        protected float MaxCooldownTime;
        
        protected int Damage;
        protected int DecayPerSecond;

        private const float OneSecond = 1;
        private ICurrentSource _currentProvider;
        private float _decayCooldownTimeLeft;
        private float _buildingTimeLeft;
        private float _stunnedTimeLeft;
        private int _currentHealth;


        public void Initialize(SentryConfig config, IMobSpawnerService mobSpawnerService, IGameFactory gameFactory)
        {
            Config = config;
            MobSpawnerService = mobSpawnerService;
            GameFactory = gameFactory;
            
            MaxHealth = config.MaxDurability;
            DecayPerSecond = config.DecayPerSecond;
            _decayCooldownTimeLeft = OneSecond; 
            TakeHealth(MaxHealth);
            
            Damage = Config.Damage;
            
            MaxCooldownTime = config.TimeBetweenActions;
            ActionCooldownTimeLeft = 0f;

            _buildingTimeLeft = mobSpawnerService.IsWaveOngoing ? config.SecondsToBuild : 0f;
        }

        protected abstract void PerformAction();

        protected virtual void Update()
        {
            if (_buildingTimeLeft > 0)
            {
                _buildingTimeLeft -= Time.deltaTime;
                return;
            }

            if (CurrentSource == null || !CurrentSource.IsActive)
                return;

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
            if (LockedTarget != null && Vector3.Distance(LockedTarget.transform.position, transform.position) <= scanRadius)
                return;


            LockedTarget = MobSpawnerService.CurrentlyAliveMobs
                .Where(x => Vector3.Distance(transform.position, x.transform.position) <= scanRadius)
                .OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude)
                .FirstOrDefault()?
                .transform;
        }

        private void KeepDecay()
        {
            //they wont decay without power (to my vision)
            if (_decayCooldownTimeLeft > 0f)
            {
                _decayCooldownTimeLeft -= Time.deltaTime;
                return;
            }
            
            _decayCooldownTimeLeft = OneSecond;
            TakeDamage(DecayPerSecond);
        }

        private void DestroyBuilding()
            => Destroy(gameObject);

        protected virtual void KeepTrackingEnemy()
        {
            if (LockedTarget == null)
                return;
            
            Vector3 direction = LockedTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float AngleOffset = 90f;
            rotatingPart.rotation = Quaternion.Euler(0, 0, angle + AngleOffset);
        }

        public ICurrentSource CurrentSource => _currentProvider;

        public float Consumption => Config.PowerConsumption;

        public LineRenderer Wire { get; set; }

        public void AttachToSource(ICurrentSource currentProvider)
        {
            _currentProvider = currentProvider;
            if(currentProvider != null)
                _currentProvider.OnOverload += HandleOverload;
        }

        private void HandleOverload(object sender, EventArgs e) 
            => _stunnedTimeLeft = Config.StunTime;

        public void TakeDamage(int damage) 
            => CurrentHealth -= damage;

        public void TakeHealth(int medicine) 
            => CurrentHealth += medicine;
    }
}