using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class FanaticStateMachine : MobStateMachine
    {
        public FanaticStateMachine(MobCombat mobCombat, MobAnimator animator, GameObject player)
            : base(mobCombat, animator)
        {
            var bladeBoxCollider = mobCombat.GetComponentInChildren<MobAttackArea>().Collider;

            States = new()
            {
                [typeof(FanaticChaseState)] = new FanaticChaseState(this, player, mobCombat),
                [typeof(FanaticChargeJump)] = new FanaticChargeJump(this, player, mobCombat),
                [typeof(FanaticPerformAttack)] = new FanaticPerformAttack(this, player, mobCombat, bladeBoxCollider)
            };
        }
    }
}