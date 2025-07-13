using System;
using UnityEngine;

namespace HighVoltage.Level
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Config/LevelConfig", order = 2)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int levelID = 0;
        [SerializeField] private LevelEnemy[] enemies;
        [SerializeField] private float deltaBetweenSpawns;

        public int LevelID => levelID;
        public LevelEnemy[] Enemies => enemies;
        public float DeltaBetweenSpawns => deltaBetweenSpawns;
    }

    [Serializable]
    public class LevelEnemy
    {
        [SerializeField] private int quantity;
        [SerializeField] private int enemyID;

        public int Quantity => quantity;
        public int EnemyID => enemyID;
    }
}