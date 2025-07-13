using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class TankDealDamage : IMobState
    {
        private readonly TankStateMachine _stateMachine;
        private readonly BoxCollider _attackCollider;
        private float _spinTime = 3f;
        private float _spinTimeElapsed = 0f;

        public TankDealDamage(TankStateMachine tankStateMachine, BoxCollider attackCollider)
        {
            _stateMachine = tankStateMachine;
            _attackCollider = attackCollider;
        }

        public void Enter() 
            => _attackCollider.enabled = true;

        public void Exit() 
            => _attackCollider.enabled = false;

        public void Update()
        {
            if (_spinTimeElapsed > _spinTime)
            {
                _stateMachine.Enter<TankChaseState>();
                _spinTimeElapsed = 0f;
                return;
            }

            _spinTimeElapsed += Time.deltaTime;
        }
    }
}