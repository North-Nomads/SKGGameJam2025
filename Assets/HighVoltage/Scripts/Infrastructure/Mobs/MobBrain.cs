using System;
using System.IO;
using HighVoltage.Enemy;
using HighVoltage.Infrastructure.Sentry;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

namespace HighVoltage.Infrastructure.Mobs
{
    public class MobBrain : MonoBehaviour, IHealthOwner
    {
        public event EventHandler<MobBrain> OnMobDied = delegate { };
        public event EventHandler<MobBrain> OnMobHitCore = delegate { };
        public event EventHandler<float> NotifyHealthBar = delegate { };

        public MobConfig Config => _config;
        
        private const float MinDistanceToWaypoint = 0.01f;
        private MobConfig _config;
        private Transform[] _waypoints;
        private Transform _target;
        private int _waypointIndex;        
        private int _currentHealth;

        public int MaxHealth { get; private set; }

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                NotifyHealthBar(this, (float)_currentHealth / MaxHealth);
                if (_currentHealth <= 0)
                    HandleMobDeath();
            }
        }

        public int CoreDamage => _config.Damage;

        private void HandleMobDeath(bool sendEvent = true)
        {
            Destroy(gameObject);
            if (sendEvent)
                OnMobDied(this, this);
        }
        
        public void TakeHealth(int medicine) 
            => CurrentHealth += medicine;

        public void TakeDamage(int damage)
        {
            if (damage == int.MaxValue)
            {
                OnMobHitCore(this, this);
                HandleMobDeath(sendEvent: false);
            }
            else
            {
                CurrentHealth -= damage;
            }
        }

        public void Initialize(Transform[] waypoints, MobConfig config)
        {
            _waypoints = waypoints;
            _target = waypoints[0];
            _waypointIndex = 0;
            _config = config;
            MaxHealth = config.MaxHealth;
            TakeHealth(config.MaxHealth);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _target.position) <= MinDistanceToWaypoint)
                MoveToNextTargetWaypoint();
            else
                transform.Translate(Time.deltaTime * _config.MoveSpeed * (_target.position - transform.position).normalized);
        }

        private void MoveToNextTargetWaypoint()
        {
            _waypointIndex++;
            if (_waypointIndex >= _waypoints.Length)
                return;
            _target = _waypoints[_waypointIndex];
        }
    }
}