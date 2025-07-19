using UnityEngine.UI;

namespace HighVoltage.Infrastructure.Sentry
{
    public interface IHealthOwner
    {
        int MaxHealth { get; }
        int CurrentHealth { get; }
        Image HealthBarFiller { get; } 
        void UpdateHealthBar();
        void TakeDamage(int damage);
        void TakeHealth(int medicine);
    }
}