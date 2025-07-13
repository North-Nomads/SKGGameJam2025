using System;

namespace HighVoltage.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int CurrentLevel;
        public int RemainingTasks;

        public bool IsLastLevel => RemainingTasks <= 0;

        public PlayerProgress(int defaultWeaponID)
        {
            CurrentLevel = 1;
            RemainingTasks = 4;
        }

        public override string ToString() 
            => $"Level={CurrentLevel};LevelsRemain={RemainingTasks}";
    } 
}
