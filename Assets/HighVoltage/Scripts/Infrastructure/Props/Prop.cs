using UnityEngine;
using HighVoltage.Infrastructure.Mobs;

namespace HighVoltage.Infrastructure.Interactables
{
    [RequireComponent(typeof(Collider))]
    public class Prop : MonoBehaviour
    {
        [SerializeField] private float minImpulse;
        [SerializeField] private float minDamage;
        [SerializeField] private float damageCoefficient;
        [SerializeField] private Collider interactionBox;

        private void OnValidate()
        {
            interactionBox = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.TryGetComponent<MobCombat>(out var mob))
                return;
            float impulse = collision.impulse.magnitude;
            Debug.Log($"Applied impulse: {impulse}");
            if (impulse > minImpulse)
            {
                mob.ApplyDamage(minDamage + damageCoefficient * (impulse - minImpulse));
            }
        }
    }
}