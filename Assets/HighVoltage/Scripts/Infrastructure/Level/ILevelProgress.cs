using System;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Level
{
    public interface ILevelProgress : IService
    {
        bool RewardGranted { get; }

        event EventHandler<bool> LevelFinishedWithReward;
        void UpdateOnNewLevel(LevelTaskConfig taskConfig);
    }
}