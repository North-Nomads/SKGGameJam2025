using System;
using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class FanaticChargeJump : IMobState
    {
        private readonly MobCombat _mobCombat;
        private readonly GameObject _player;
        private readonly GameObject _self;
        private readonly Rigidbody _rigidBody;
        private FanaticStateMachine _stateMachine;
        private float _totalTimeToWait = 2f;
        private float _waitingTimePassed;
        private float rotationSpeed = 1f;

        public FanaticChargeJump(FanaticStateMachine fanaticStateMachine, GameObject player, MobCombat mobCombat)
        {
            _player = player;
            _mobCombat = mobCombat;
            _rigidBody = mobCombat.GetComponent<Rigidbody>();
            _self = mobCombat.gameObject;
            _stateMachine = fanaticStateMachine;
        }

        public void Enter()
        {
            _rigidBody.velocity = Vector3.zero;
        }

        public void Exit()
        {
            _waitingTimePassed = 0f;
        }

        public void Update()
        {
            LookAtPlayer();

            if (_waitingTimePassed <= _totalTimeToWait)
            {
                _waitingTimePassed += Time.deltaTime;
                return;
            }

            _stateMachine.Enter<FanaticPerformAttack>();
        }

        private void LookAtPlayer()
        {
            if (_player == null)
                return;
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