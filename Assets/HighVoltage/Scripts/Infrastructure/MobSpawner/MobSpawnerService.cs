using System;
using System.Collections;
using System.Collections.Generic;
using HighVoltage.Enemy;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Level;
using HighVoltage.StaticData;

namespace HighVoltage.Infrastructure.MobSpawning
{
    public class MobSpawnerService : IMobSpawnerService
    {
        public List<MobBrain> CurrentlyAliveMobs => _currentlyAliveMobs;
        public event EventHandler<int> AnotherMobDied = delegate { };
        
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameFactory _factory;
        
        private List<MobBrain> _currentlyAliveMobs = new();
        private List<Coroutine> _runningGateCoroutines = new();

        public MobSpawnerService(IGameFactory factory, IStaticDataService staticDataService, ICoroutineRunner coroutineRunner)
        {
            _factory = factory;
            _coroutineRunner = coroutineRunner;
            _staticDataService = staticDataService;
        }
        
        public void LoadConfigToSpawners(MobWave levelConfig, WaypointHolder[] spawnerSpots, float deltaBetweenSpawns)
        {
            _currentlyAliveMobs = new List<MobBrain>();
            foreach (Coroutine runningGateCoroutine in _runningGateCoroutines) 
                _coroutineRunner.StopCoroutine(runningGateCoroutine);
            _runningGateCoroutines = new List<Coroutine>();

            for (int gateIndex = 0; gateIndex < levelConfig.Gates.Length; gateIndex++)
            {
                Coroutine coroutine = _coroutineRunner.StartCoroutine(SpawnGateCoroutine(levelConfig.Gates[gateIndex],
                    spawnerSpots[gateIndex], deltaBetweenSpawns));
                _runningGateCoroutines.Add(coroutine);
            }
        }

        public void HandleMobReachedCore(MobBrain mob)
        {
            mob.HandleHit(int.MaxValue);
            // Handle core damage
        }

        private IEnumerator SpawnGateCoroutine(Gate gate, WaypointHolder spawnerSpot, float deltaBetweenSpawns)
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
                    yield return new WaitForSeconds(deltaBetweenSpawns);
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