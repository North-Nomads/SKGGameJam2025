using UnityEngine;
using UnityEngine.Serialization;

namespace HighVoltage.Infrastructure.Sentry
{
    [CreateAssetMenu(fileName = "SentryConfig", menuName = "Config/Sentry Config", order = 2)]
    public class SentryConfig : ScriptableObject
    {
        [SerializeField] private float timeBetweenActions;
        [SerializeField] private int maxDurability;
        [SerializeField] private int damage;
        [SerializeField] private int decayPerSecond;
        [SerializeField] private int sentryId;
        [SerializeField] private int bulletsPerAction = 1;
        [SerializeField] private float bulletsAngleOffset = 1;

        public float BulletsAngleOffset => bulletsAngleOffset;

        public int BulletsPerAction => bulletsPerAction;

        public float TimeBetweenActions => timeBetweenActions;

        public int MaxDurability => maxDurability;
        public int Damage => damage;

        public int DecayPerSecond => decayPerSecond;

        public int SentryId => sentryId;
    }
}