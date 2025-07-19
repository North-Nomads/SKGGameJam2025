using System;

namespace HighVoltage.Infrastructure.Sentry
{
    public interface IHealthOwner
    {
        event EventHandler<float> NotifyHealthBar;  
        int MaxHealth { get; }
        int CurrentHealth { get; }
        void TakeDamage(int damage);
        void TakeHealth(int medicine);
    }
}