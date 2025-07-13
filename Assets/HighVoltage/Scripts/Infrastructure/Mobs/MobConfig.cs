using UnityEngine;
using HighVoltage.Infrastructure.Mobs;

namespace HighVoltage.Enemy
{
    [CreateAssetMenu(fileName = "DefaultMob", menuName = "Config/Enemy Config", order = 1)]
    public class MobConfig : ScriptableObject
    {
        public int EnemyId => enemyId;
        public MobBrain EnemyPrefab => enemyPrefab;
        public int MaxHealth => maxHealth;
        public int Damage => damage;
        public int Reward => reward;
        public float MoveSpeed => moveSpeed;

        [SerializeField] private int enemyId;
        [SerializeField] private MobBrain enemyPrefab;
        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;
        [SerializeField] private int reward;
        [SerializeField] private float moveSpeed;
        
    }
}