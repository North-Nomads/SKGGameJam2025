using System.Collections;
using UnityEngine;

namespace HighVoltage.Infrastructure.Spells
{
    public class BlackHoleSpell : MonoBehaviour
    {
        [Header("Combat")]
        [SerializeField, Min(1)] private int totalTicks;
        [SerializeField] private float damagePerTick;

        [Header("Physics")]
        [SerializeField] private float pullRadius = 5f; 
        [SerializeField] private float pullDuration = 3f;
        [SerializeField] private LayerMask layerMask;

        private Collider[] _targets = new Collider[50];

        private void Awake() 
            => StartCoroutine(PullObjectsCoroutine());

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, pullRadius);  
        }

        private IEnumerator PullObjectsCoroutine()
        {
            float elapsedTime = 0f;

            float timeToOneTick = pullDuration / totalTicks;
            float timeLeftToDealDamage = 0f;
            int objectsFound;

            while (elapsedTime < pullDuration)
            {
                objectsFound = Physics.OverlapSphereNonAlloc(transform.position, pullRadius, _targets, layerMask);

                ParseAffectedItems(timeLeftToDealDamage, timeToOneTick, objectsFound);

                if (timeLeftToDealDamage <= 0f)
                    timeLeftToDealDamage = timeToOneTick;

                elapsedTime += Time.deltaTime;
                timeLeftToDealDamage -= Time.deltaTime;

                yield return null;
            }

            Destroy(gameObject);
        }

        private void ParseAffectedItems(float timeToDealDamageLeft, float timeToOneTick, int objectsFound)
        {
            for (int i = 0; i < objectsFound; i++)
            {
                PullPullableItems(_targets[i]);
                HitHittableItems(_targets[i]);
            }

            void PullPullableItems(Collider collider)
            {
                if (collider.TryGetComponent<Rigidbody>(out var rb))
                {
                    Vector3 direction = (transform.position - rb.position).normalized;
                    rb.AddForce(direction * 10f); // Применяем силу притяжения
                }
            }

            void HitHittableItems(Collider collider)
            {
                if (timeToDealDamageLeft <= 0f)
                {
                    if (collider.TryGetComponent<IHittable>(out var hittable))
                        hittable.ApplyDamage(damagePerTick);
                }
            }
        }
    }
}