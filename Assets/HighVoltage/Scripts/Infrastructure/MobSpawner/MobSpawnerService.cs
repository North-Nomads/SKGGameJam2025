using System;
using System.Collections;
using System.Collections.Generic;
using HighVoltage.Enemy;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Level;
using HighVoltage.StaticData;
using Unity.VisualScripting;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public class MobSpawnerService : IMobSpawnerService
    {
        public List<MobBrain> CurrentlyAliveMobs => _currentlyAliveMobs;
        public event EventHandler<int> AnotherMobDied = delegate { };
        
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<MobBrain> _currentlyAliveMobs;
        private readonly IGameFactory _factory;

        public MobSpawnerService(IGameFactory factory, IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _factory = factory;
            _coroutineRunner = coroutineRunner;
            _staticDataService = staticDataService;
            _currentlyAliveMobs = new List<MobBrain>();
        }
        
        public void LoadConfigToSpawners(LevelConfig levelConfig, WaypointHolder[] spawnerSpots)
        {
            for (int gateIndex = 0; gateIndex < levelConfig.Gates.Length; gateIndex++)
            {
                _coroutineRunner.StartCoroutine(SpawnGateCoroutine(levelConfig.Gates[gateIndex],
                    spawnerSpots[gateIndex], levelConfig));
            }
        }

        public void HandleMobReachedCore(MobBrain mob)
        {
            mob.HandleHit(int.MaxValue);
            // Handle core damage
        }

        private IEnumerator SpawnGateCoroutine(Gate gate, WaypointHolder spawnerSpot, LevelConfig levelConfig)
        {
            int mobNameIndex = 0;

            foreach (EnemyEntry entry in gate.LevelEnemies)
            {
                MobConfig mobConfig = _staticDataService.ForEnemyID(entry.EnemyID);

                for (int i = 0; i < entry.Quantity; i++)
                {
                    SpawnMob(
                        mobConfig,
                        spawnerSpot.Waypoints[0].position,
                        spawnerSpot.Waypoints,
                        mobNameIndex
                    );

                    mobNameIndex++;

                    yield return new WaitForSeconds(levelConfig.DeltaBetweenSpawns);
                }
            }
        }
        
        private void SpawnMob(MobConfig which, Vector3 where, Transform[] pathFromGate, int nameIndex)
        {
            MobBrain mob = _factory.CreateMobOn(which.EnemyPrefab, where);
            mob.Initialize(pathFromGate, which);
            mob.OnMobDied += HandleMobDeath;
            mob.name = $"{which.name} [{nameIndex}]";
            _currentlyAliveMobs.Add(mob);
        }

        private void HandleMobDeath(object sender, MobBrain mob)
        {
            _currentlyAliveMobs.Remove(mob);
            AnotherMobDied(null, _currentlyAliveMobs.Count);
        }
    }
}