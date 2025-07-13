namespace HighVoltage.Infrastructure.Mobs
{

    public class FanaticMobBrain : MobBrain
    {
        protected override void OnStart()
        {
            StateMachine = new FanaticStateMachine(MobCombat, MobAnimator, MobSpawner.PlayerInstance);
            StateMachine.Enter<FanaticChaseState>();
        }
    }
}