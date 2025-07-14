using System;
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
        private readonly IStaticDataService _staticDataService;
        private readonly List<MobBrain> _currentlyAliveMobs;
        private readonly IGameFactory _factory;

        public event EventHandler<int> AnotherMobDied = delegate { };

        public MobSpawnerService(IGameFactory factory, IStaticDataService staticDataService)
        {
            _factory = factory;
            _staticDataService = staticDataService;
            _currentlyAliveMobs = new List<MobBrain>();
        }
        
        public void LoadConfigToSpawners(LevelConfig levelConfig, WaypointHolder[] spawnerSpots)
        {
            int gateIndex = 0;
            int mobNameIndex = 0;

            foreach (Gate gate in levelConfig.Gates)
            {
                foreach (EnemyEntry enemy in gate.LevelEnemies)
                {
                    MobConfig mobConfig = _staticDataService.ForEnemyID(enemy.EnemyID);
                    for (int i = 0; i < enemy.Quantity; i++)
                    {
                        SpawnMob(mobConfig, spawnerSpots[gateIndex].Waypoints[0].position,
                            spawnerSpots[gateIndex].Waypoints, mobNameIndex);
                        mobNameIndex++;
                    }
                }
                gateIndex++;
            }
        }

        public void HandleMobReachedCore(MobBrain mob)
        {
            mob.HandleHit(int.MaxValue);
            // Handle core damage
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