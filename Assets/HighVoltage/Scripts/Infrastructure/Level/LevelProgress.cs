using System;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;

namespace HighVoltage.Level
{
    public class LevelProgress : ILevelProgress
    {
        public event EventHandler<bool> LevelFinishedWithReward = delegate { };

        private readonly IMobSpawnerService _mobSpawner;
        private LevelTaskConfig _taskConfig;

        private bool _noDamageThisLevel;
        private float _levelStartTime;

        private bool _hasRewardToGrant;

        public bool RewardGranted => _hasRewardToGrant;

        public void UpdateOnNewLevel(LevelTaskConfig taskConfig)
        {
            _taskConfig = taskConfig;
            _noDamageThisLevel = true;
            _levelStartTime = Time.time;
        }

        public LevelProgress(IMobSpawnerService mobSpawner)
        {
            _mobSpawner = mobSpawner;
            _mobSpawner.AnotherMobDied += HandleMobDeath;
        }

        private void HandleMobDeath(object sender, int mobsLeft)
        {
            if (mobsLeft != 0)
                return;

            bool grantReward = CheckLevelTaskConditions();
            LevelFinishedWithReward(null, grantReward);
            _hasRewardToGrant = grantReward;
        }

        private bool CheckLevelTaskConditions()
        {
            if (_taskConfig.NoDamageChallenge)
                if (_taskConfig.NoDamageChallenge != _noDamageThisLevel)
                    return false;

            var levelCompleteTime = Time.time - _levelStartTime;
            if (_taskConfig.SecondsToComplete > 0)
                if (levelCompleteTime > _taskConfig.SecondsToComplete)
                    return false;

            return true;
        }
    }
}