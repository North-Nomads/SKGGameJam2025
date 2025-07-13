using System;
using System.Collections.Generic;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.Mobs;

namespace HighVoltage.Infrastructure.MobSpawnerService
{
    public class MobSpawner : IMobSpawnerService
    {
        private readonly IGameFactory _factory;
        private readonly List<MobBrain> _currentlyAliveMobs;
        private GameObject _playerInstance;

        public GameObject PlayerInstance => _playerInstance;

        public event EventHandler<int> AnotherMobDied = delegate { };

        public MobSpawner(IGameFactory factory)
        {
            _factory = factory;
            _currentlyAliveMobs = new();
        }

        public void SpawnMobs(GameObject player)
        {
            _currentlyAliveMobs.Clear();
            _playerInstance = player;

            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("MobSpawnPoint");
            foreach (var spawnPoint in spawnPoints)
            {
                MobBrain mob = _factory.CreateMobOn(spawnPoint);
                mob.OnMobDied += HandleMobDeath;
                _currentlyAliveMobs.Add(mob);
            }
        }

        private void HandleMobDeath(object sender, MobBrain mob)
        {
            _currentlyAliveMobs.Remove(mob);
            AnotherMobDied(null, _currentlyAliveMobs.Count);
        }
    }
}