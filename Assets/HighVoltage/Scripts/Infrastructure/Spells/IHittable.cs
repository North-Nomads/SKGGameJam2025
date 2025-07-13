namespace HighVoltage.Infrastructure.Spells
{
    internal interface IHittable
    {
        void ApplyDamage(float damagePerTick);
    }
}