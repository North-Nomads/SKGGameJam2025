using System;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.StaticData;

namespace HighVoltage.Infrastructure.BuildingStore
{
    public class BuildingStoreService : IBuildingStoreService
    {
        public event EventHandler<int> CurrencyChanged = delegate { };
        private int _moneyPlayerHas;

        public int MoneyPlayerHas => _moneyPlayerHas;

        public bool CanAfford(SentryConfig sentryConfig)
            => _moneyPlayerHas >= sentryConfig.BuildPrice;

        public void SpendMoneyOn(SentryConfig sentryConfig)
        {
            _moneyPlayerHas -= sentryConfig.BuildPrice;
            CurrencyChanged(this, _moneyPlayerHas);
        }

        public void AddMoney(int reward)
        {
            _moneyPlayerHas += reward;
            CurrencyChanged(this, _moneyPlayerHas);
        }
    }
}