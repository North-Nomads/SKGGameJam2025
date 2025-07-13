using System;
using UnityEngine;
using HighVoltage.Infrastructure.Spells;

namespace HighVoltage.Infrastructure.Mobs
{
    public class MobCombat : MonoBehaviour, IHittable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float maxHealth;
        private float _currentHealth;

        public float CurrentHealth
        {
            get
            {
                return _currentHealth;
            }
            set
            {
                _currentHealth = value;
                if (_currentHealth <= 0)
                {
                    _currentHealth = 0;
                    MobHealthBelowZero(null, null);
                }
            }
        }

        public float Speed => moveSpeed;

        public EventHandler MobHealthBelowZero = delegate { };

        private void Awake()
        {
            _currentHealth = maxHealth;    
        }

        public void ApplyDamage(float damage)
        {
            Debug.Log(damage);
            CurrentHealth -= damage;
        }
    }
}