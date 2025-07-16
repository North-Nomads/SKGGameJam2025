using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage
{
    public class PlayerCore : Building, ICurrentSource
    {
        private readonly List<ICurrentReceiver> _receivers = new();
        private IMobSpawnerService _mobSpawner;
        public event EventHandler<int> OnCoreHealthChanged = delegate { };
        public event EventHandler OnOverload;

        private int _maxCoreHealth;

        public int MaxCoreHealth => _maxCoreHealth;

        private int _currentCoreHealth;

        public int CurrentCoreHealth
        {
            get => _currentCoreHealth;
            set
            {
                _currentCoreHealth = Mathf.Clamp(value, 0, _maxCoreHealth);
                Debug.Log($"Core was damaged. Calling \"OnCoreHealthChanged\" with {_currentCoreHealth}");
                OnCoreHealthChanged(this, _currentCoreHealth);
            }
        }

        public IEnumerable<ICurrentReceiver> Receivers => _receivers;

        private float _currentGeneration;
        private float _currentCurrentOutput;
        public float Output => _currentCurrentOutput;

        public void Initialize(IMobSpawnerService mobSpawner, LevelConfig levelConfig)
        {
            _mobSpawner = mobSpawner;
            _maxCoreHealth = levelConfig.CoreHealth;
            _currentGeneration = levelConfig.GeneratorCapacity;
            CurrentCoreHealth = levelConfig.CoreHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            print($"Other trigger entered: {other.name}");
            if (other.CompareTag(Constants.MobTag))
            {
                MobBrain mobBrain = other.GetComponent<MobBrain>();
                _mobSpawner.HandleMobReachedCore(mobBrain);
                CurrentCoreHealth -= mobBrain.CoreDamage;
            }
        }

        public void AttachReceiver(ICurrentReceiver receiver)
        {
            _receivers.Add(receiver);
        }

        private void Update()
        {
            _currentCurrentOutput = _currentGeneration;
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
    }
}
