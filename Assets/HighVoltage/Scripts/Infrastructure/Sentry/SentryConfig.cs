using System;
using UnityEditor;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    [CreateAssetMenu(fileName = "SentryConfig", menuName = "Config/Sentry Config", order = 2)]
    public class SentryConfig : BuildingConfig
    {
        [Header("Combat settings")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float timeBetweenActions;
        [SerializeField] private int maxDurability;
        [SerializeField] private int damage;
        [SerializeField] private int decayPerSecond;
        [SerializeField] private int bulletsPerAction = 1;
        [SerializeField] private float bulletsAngleOffset = 1;
        [Header("Power")]
        [SerializeField] private float powerConsumption;
        [SerializeField] private float stunTime;
        [SerializeField] private int reward;

        private SentryTower _sentryPrefab; 
        
        private void Awake() 
            => _sentryPrefab = prefab.GetComponent<SentryTower>();

        public new SentryTower Prefab => _sentryPrefab;
        
        public float BulletsAngleOffset => bulletsAngleOffset;

        public int BulletsPerAction => bulletsPerAction;

        public float TimeBetweenActions => timeBetweenActions;

        public int MaxDurability => maxDurability;
        
        public int Damage => damage;

        public int DecayPerSecond => decayPerSecond;
        
        public float PowerConsumption => powerConsumption;

        public float StunTime => stunTime;

        public int Reward => reward;
    }
}