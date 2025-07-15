using System;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage
{
    public class PlayerCore : Building
    {
        private IMobSpawnerService _mobSpawner;
        public event EventHandler<int> OnCoreHealthChanged = delegate { };
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

        public void Initialize(IMobSpawnerService mobSpawner, LevelConfig levelConfig)
        {
            _mobSpawner = mobSpawner;
            _maxCoreHealth = levelConfig.CoreHealth;
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
    }
}
