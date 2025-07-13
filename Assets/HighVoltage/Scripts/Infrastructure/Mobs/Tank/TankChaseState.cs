using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class TankChaseState : IMobState
    {
        private readonly TankStateMachine _stateMachine;
        private readonly MobCombat _mobCombat;
        private readonly GameObject _player;
        private readonly Rigidbody _rigidbody;
        private readonly GameObject _self;
        private GameObject _chaseTarget;

        private float _chaseRadius = 3f;
        private float _chargeReloadTimePassed = 0f;
        private float _chargeReloadTime = 1f;
        private float rotationSpeed = 1f;

        public TankChaseState(TankStateMachine tankStateMachine, MobCombat mobCombat, GameObject player)
        {
            _stateMachine = tankStateMachine;
            _mobCombat = mobCombat;
            _player = player;
            _self = mobCombat.gameObject;
            _rigidbody = mobCombat.GetComponent<Rigidbody>();
        }

        public void Enter()
        {
            _chaseTarget = _player;
        }
        public void Exit()
        {
            _chargeReloadTimePassed = 0f;
        }

        public void Update()
        {
            LookAtPlayer();
            float distanceToTarget = Vector3.Distance(_self.transform.position, _chaseTarget.transform.position);
            _chargeReloadTimePassed += Time.deltaTime;

            if (distanceToTarget > _chaseRadius)
            {
                Vector3 direction = (_chaseTarget.transform.position - _self.transform.position).normalized;
                _rigidbody.AddForce(direction * _mobCombat.Speed);
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _mobCombat.Speed);
            }
            else
            {
                if (_chargeReloadTimePassed <= _chargeReloadTime)
                    return;

                _stateMachine.Enter<TankDealDamage>();
            }
        }

        private void LookAtPlayer()
        {
            Vector3 directionToPlayer = _player.transform.position - _self.transform.position;
            directionToPlayer.y = 0;

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                _self.transform.rotation = Quaternion.Slerp(_self.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}