using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class FanaticPerformAttack : IMobState
    {
        private const float StopThreshold = 0.01f;

        private readonly FanaticStateMachine _stateMachine;
        private readonly MobCombat _mobCombat;
        private readonly Rigidbody _rigidBody;
        private BoxCollider _attackCollider;
        private float _attackTimeElapsed;
        private float _attackTimeTotal = .3f;

        public FanaticPerformAttack(FanaticStateMachine fanaticStateMachine, GameObject player, MobCombat mobCombat, BoxCollider attackCollider)
        {
            _stateMachine = fanaticStateMachine;
            _mobCombat = mobCombat;
            _rigidBody = _mobCombat.GetComponent<Rigidbody>();
            _attackCollider = attackCollider;
        }

        public void Enter()
        {
            _attackTimeElapsed = 0f;
            _rigidBody.AddForce(_mobCombat.transform.forward * 10f, ForceMode.VelocityChange);
            _attackCollider.enabled = true;
        }

        public void Exit()
            => _attackCollider.enabled = false;

        public void Update()
        {
            if (IsAttackFinished())
                _stateMachine.Enter<FanaticChaseState>();

            _attackTimeElapsed += Time.deltaTime;
        }

        private bool IsAttackFinished() 
            => _attackTimeElapsed >= _attackTimeTotal;
    }
}