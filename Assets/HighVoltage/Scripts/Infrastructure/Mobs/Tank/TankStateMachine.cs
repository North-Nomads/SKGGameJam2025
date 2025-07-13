using UnityEngine;

namespace HighVoltage.Infrastructure.Mobs
{
    public class TankStateMachine : MobStateMachine
    {
        public TankStateMachine(MobCombat mobCombat, MobAnimator animator, GameObject player) : base(mobCombat, animator)
        {
            var bladeBoxCollider = mobCombat.GetComponentInChildren<MobAttackArea>().Collider;

            States = new()
            {
                [typeof(TankChaseState)] = new TankChaseState(this, mobCombat, player),
                [typeof(TankDealDamage)] = new TankDealDamage(this, bladeBoxCollider),
            };
        }
    }
}