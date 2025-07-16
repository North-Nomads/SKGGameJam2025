using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    [CreateAssetMenu(fileName = "SentryConfig", menuName = "Config/Sentry Config", order = 2)]
    public class SentryConfig : ScriptableObject
    {
        [SerializeField] private SentryTower sentryPrefab;
        [SerializeField] private Bullet bulletPrefab;
        [Header("Combat settings")]
        [SerializeField] private float timeBetweenActions;
        [SerializeField] private int maxDurability;
        [SerializeField] private int damage;
        [SerializeField] private int decayPerSecond;
        [SerializeField] private int sentryId;
        [SerializeField] private int bulletsPerAction = 1;
        [SerializeField] private float bulletsAngleOffset = 1;
        [Header("Power")]
        [SerializeField] private float powerConsumption;
        [SerializeField] private float stunTime;
        [Header("UI settings")]
        [SerializeField] private Sprite sentryIcon;
        [SerializeField] private int buildPrice;

        public float BulletsAngleOffset => bulletsAngleOffset;

        public int BulletsPerAction => bulletsPerAction;

        public float TimeBetweenActions => timeBetweenActions;

        public int MaxDurability => maxDurability;
        public int Damage => damage;

        public int DecayPerSecond => decayPerSecond;

        public int SentryId => sentryId;

        public Sprite SentryIcon => sentryIcon;

        public int BuildPrice => buildPrice;

        public SentryTower SentryPrefab => sentryPrefab;

        public float PowerConsumption => powerConsumption;

        public float StunTime => stunTime;
    }
}