namespace HighVoltage.Infrastructure.Mobs
{
    public interface IMobState
    {
        void Exit();
        void Enter();
        void Update();
    }
}