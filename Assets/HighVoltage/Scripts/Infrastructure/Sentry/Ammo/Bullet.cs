using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private float maxRadius;
        [SerializeField] private float bulletSpeed;
        protected int BulletDamage;
        protected Vector3 MoveDirection;
        protected Vector3 InitialPosition;

        public void Initialize(Vector3 targetPosition, int damage)
        {
            MoveDirection = (targetPosition - transform.position).normalized;
            BulletDamage = damage;
            InitialPosition = transform.position;
        }


        private void Update()
        {
            transform.Translate(Time.deltaTime * bulletSpeed * MoveDirection);
            if (Vector3.Distance(transform.position, InitialPosition) > maxRadius)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Constants.MobTag))
                return;

            HandleContact(other);
        }

        protected abstract void HandleContact(Collider2D mob);
    }
}