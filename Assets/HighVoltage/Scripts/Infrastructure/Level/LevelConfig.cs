using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HighVoltage.Level
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Config/LevelConfig", order = 2)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int levelID = 0;
        [SerializeField] private float deltaBetweenSpawns;
        [SerializeField] private Gate[] gates;
        [SerializeField] private int coreHealth;
        [SerializeField] private List<int> sentryIDs;

        public List<int> SentryIDs => sentryIDs;
        public int LevelID => levelID;
        public float DeltaBetweenSpawns => deltaBetweenSpawns;
        public Gate[] Gates => gates;
        public int CoreHealth => coreHealth;
    }

    [Serializable]
    public class Gate
    {
        [SerializeField] private EnemyEntry[] levelEnemies;

        public EnemyEntry[] LevelEnemies => levelEnemies;
    }

    [Serializable]
    public class EnemyEntry
    {
        [SerializeField] private int quantity;
        [SerializeField] private int enemyID;

        public int Quantity => quantity;
        public int EnemyID => enemyID;
    }
}