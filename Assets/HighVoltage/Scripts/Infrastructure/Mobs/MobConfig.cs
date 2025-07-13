using UnityEngine;
using HighVoltage.Infrastructure.Mobs;

namespace HighVoltage.Enemy
{
    [CreateAssetMenu(fileName = "Zombie_", menuName = "Config/Enemy Config", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private int enemyId;
        [SerializeField] private GameObject enemyPrefab;
        [Header("Combat properties")]
        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;
        [SerializeField] private int reward;
        [SerializeField] private float moveSpeed;

        public int Reward => reward;
        public int Damage => damage;
        public int MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public int EnemyId => enemyId;
        public GameObject Prefab => enemyPrefab;
    }
}