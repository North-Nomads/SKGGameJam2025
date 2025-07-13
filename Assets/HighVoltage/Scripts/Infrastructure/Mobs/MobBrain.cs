using System;
using HighVoltage.Enemy;
using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class MobBrain : MonoBehaviour
    {
        private Transform _target;
        private MobConfig _config;
        public event EventHandler<MobBrain> OnMobDied = delegate { };

        public void Initialize(GameObject attackTarget, MobConfig config)
        {
            _target = attackTarget.transform;
            _config = config;
        }

        private void Update()
        {
            transform.Translate((_target.position - transform.position).normalized * (Time.deltaTime * _config.MoveSpeed));
        }
    }
}