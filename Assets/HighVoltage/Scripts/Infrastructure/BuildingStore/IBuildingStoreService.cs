using System;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.BuildingStore
{
    public interface IBuildingStoreService : IService
    {
        event EventHandler<int> CurrencyChanged;
        int MoneyPlayerHas { get; }
        bool CanAfford(SentryConfig sentryConfig);
        void SpendMoneyOn(SentryConfig sentryConfig);
        void AddMoney(int reward);
    }
}