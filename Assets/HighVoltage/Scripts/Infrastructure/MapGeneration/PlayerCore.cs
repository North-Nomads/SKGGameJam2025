using System;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage
{
    public class PlayerCore : MonoBehaviour
    {
        private IMobSpawnerService _mobSpawner;
        
        public void Initialize(IMobSpawnerService mobSpawner)
        {
            _mobSpawner = mobSpawner;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constants.MobTag))
                _mobSpawner.HandleMobReachedCore(other.GetComponent<MobBrain>());
        }
    }
}
