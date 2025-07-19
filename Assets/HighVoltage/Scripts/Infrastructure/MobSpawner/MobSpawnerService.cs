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
        private readonly IGameFactory _factory;
        
        private List<MobBrain> _currentlyAliveMobs = new();
        private List<Coroutine> _runningGateCoroutines = new();
        
        private ICoroutineRunner _coroutineRunner;
        private WaypointHolder[] _spawnerSpots;
        private float _deltaBetweenSpawns;
        private MobWave _mobWaveConfig;
        private bool _isWaveOngoing;
        
        public bool IsWaveOngoing => _isWaveOngoing;

        public MobSpawnerService(IGameFactory factory, IStaticDataService staticDataService)
        {
            _factory = factory;
            _staticDataService = staticDataService;
        }
        
        public void LoadConfigToSpawners(MobWave waveConfig, WaypointHolder[] spawnerSpots, float deltaBetweenSpawns)
        {
            _mobWaveConfig = waveConfig;
            _spawnerSpots = spawnerSpots;
            _deltaBetweenSpawns = deltaBetweenSpawns;
            _coroutineRunner = _factory.CreateCoroutineRunner();
        }

        public void HandleMobReachedCore(MobBrain mob)
        {
            mob.TakeDamage(int.MaxValue);
            _currentlyAliveMobs.Remove(mob);
        }

        public void LaunchMobSpawning()
        {
            _currentlyAliveMobs = new List<MobBrain>();
            foreach (Coroutine runningGateCoroutine in _runningGateCoroutines)
                if (runningGateCoroutine != null)
                    _coroutineRunner.StopCoroutine(runningGateCoroutine);
            _runningGateCoroutines = new List<Coroutine>();

            for (int gateIndex = 0; gateIndex < _mobWaveConfig.Gates.Length; gateIndex++)
            {
                Coroutine coroutine = _coroutineRunner.StartCoroutine(SpawnGateCoroutine(_mobWaveConfig.Gates[gateIndex],
                    _spawnerSpots[gateIndex], _deltaBetweenSpawns));
                _runningGateCoroutines.Add(coroutine);
            }
        }

        public void UpdateWaveContent(MobWave newWave) 
            => _mobWaveConfig = newWave;

        public void UpdateWaveOngoingStatus(bool isWaveOngoing)
        {
            _isWaveOngoing = isWaveOngoing;
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
            AnotherMobDied(null, mob.Config.EnemyId);
        }
    }
}