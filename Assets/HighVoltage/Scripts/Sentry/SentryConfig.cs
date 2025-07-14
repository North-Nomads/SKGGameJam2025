using UnityEngine;

namespace HighVoltage.HighVoltage.Scripts.Sentry
{
    [CreateAssetMenu(fileName = "SentryConfig", menuName = "Config/Sentry Config", order = 2)]
    public class SentryConfig : ScriptableObject
    {
        [SerializeField] private float timeBetweenActions;
        [SerializeField] private int maxDurability;
        [SerializeField] private int damage;
        [SerializeField] private int decayPerSecond;
        [SerializeField] private int sentryId;

        public float TimeBetweenActions => timeBetweenActions;

        public int MaxDurability => maxDurability;
        public int Damage => damage;

        public int DecayPerSecond => decayPerSecond;

        public int SentryId => sentryId;
    }
}