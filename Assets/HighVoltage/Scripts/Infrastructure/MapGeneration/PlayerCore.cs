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
        private List<ICurrentReciever> _recievers;
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

        public IEnumerable<ICurrentReciever> Recievers => _recievers;

        public float Output => throw new NotImplementedException();

        public void Initialize(IMobSpawnerService mobSpawner, LevelConfig levelConfig)
        {
            _mobSpawner = mobSpawner;
            _maxCoreHealth = levelConfig.CoreHealth;
            CurrentCoreHealth = levelConfig.CoreHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constants.MobTag))
            {
                MobBrain mobBrain = other.GetComponent<MobBrain>();
                _mobSpawner.HandleMobReachedCore(mobBrain);
                CurrentCoreHealth -= mobBrain.CoreDamage;
            }
        }

        public void AttachReciever(ICurrentReciever reciever)
        {
            
        }

        public void RequestPower()
        {
            throw new NotImplementedException();
        }
    }
}
