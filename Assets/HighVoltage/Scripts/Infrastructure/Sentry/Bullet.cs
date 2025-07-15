using System;
using HighVoltage.Infrastructure.Mobs;
using HighVoltage.Services;
using UnityEngine;

namespace HighVoltage.Infrastructure.Sentry
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;
        private int _bulletDamage;
        private Vector3 _moveDirection;

        public void Initialize(Vector3 targetPosition, int damage)
        {
            _moveDirection = (targetPosition - transform.position).normalized;
            _bulletDamage = damage;
        }

        private void Update()
        {
            transform.Translate(Time.deltaTime * bulletSpeed * _moveDirection);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Constants.MobTag))
                return;
            
            other.GetComponent<MobBrain>().HandleHit(_bulletDamage);
        }
    }
}