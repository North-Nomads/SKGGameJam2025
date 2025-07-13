namespace HighVoltage.Infrastructure.Mobs
{

    public class TankBrain : MobBrain
    {
        protected override void OnStart()
        {
            StateMachine = new TankStateMachine(MobCombat, MobAnimator, MobSpawner.PlayerInstance);
            StateMachine.Enter<TankChaseState>();
        }
    }
}