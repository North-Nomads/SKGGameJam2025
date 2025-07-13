using System;
using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class MobBrain : MonoBehaviour
    {
        private Transform _target;
        public event EventHandler<MobBrain> OnMobDied = delegate { };

        public void Initialize(GameObject attackTarget) 
            => _target = attackTarget.transform;
    }
}