using System;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.BuildingStore
{
    public interface IBuildingStoreService : IService
    {
        event EventHandler<int> CurrencyChanged;
        int MoneyPlayerHas { get; }
        bool CanAfford(BuildingConfig sentryConfig);
        void SpendMoneyOn(BuildingConfig sentryConfig);
        void AddMoney(int reward);
    }
}