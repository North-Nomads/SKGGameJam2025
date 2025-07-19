using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Level;
using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage
{
    public class PlayerCore : Building, ICurrentSource, IHealthOwner
    {
        public event EventHandler<int> OnCoreHealthChanged = delegate { };
        public event EventHandler<float> NotifyHealthBar = delegate { };
        public int MaxHealth { get; private set; }
        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                NotifyHealthBar(this, (float)_currentHealth / MaxHealth);
                OnCoreHealthChanged(this, _currentHealth);
            }
        }
        
        private readonly List<ICurrentReceiver> _receivers = new();
        private IMobSpawnerService _mobSpawner;
        public event EventHandler OnOverload;

        public IEnumerable<ICurrentReceiver> Receivers => _receivers;

        private float _currentGeneration;
        private float _currentCurrentOutput;
        private int _currentHealth;
        public float Output => _currentCurrentOutput;

        public bool IsActive { get; private set; }

        public void Initialize(IMobSpawnerService mobSpawner, LevelConfig levelConfig)
        {
            _mobSpawner = mobSpawner;
            _currentGeneration = levelConfig.GeneratorCapacity;
            
            MaxHealth = levelConfig.CoreHealth;
            TakeHealth(MaxHealth);
            NotifyHealthBar(this, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Constants.MobTag)) 
                return;
            
            MobBrain mobBrain = other.GetComponent<MobBrain>();
            _mobSpawner.HandleMobReachedCore(mobBrain);
            TakeDamage(mobBrain.CoreDamage);
        }

        public void AttachReceiver(ICurrentReceiver receiver)
        {
            _receivers.Add(receiver);
        }

        private void Update()
        {
            _currentCurrentOutput = _currentGeneration;
            foreach (var receiver in Receivers)
            {
                _currentCurrentOutput -= receiver.Consumption;
            }
            IsActive = _currentCurrentOutput >= 0;
        }

        public void RequestPower(float configPowerConsumption)
        {
            _currentCurrentOutput -= configPowerConsumption;
            if (_currentCurrentOutput < 0)
                OnOverload.Invoke(this, null);
        }

        public void DetachAllReceivers()
        {
            foreach (var receiver in _receivers)
                receiver.AttachToSource(null);
            _receivers.Clear();
        }

        public void DetachReceiver(ICurrentReceiver receiver)
        {
            receiver.AttachToSource(null);
            _receivers.Remove(receiver);
        }

        public void TakeDamage(int damage) 
            => CurrentHealth -= damage;

        public void TakeHealth(int medicine) 
            => CurrentHealth += medicine;
    }
}
