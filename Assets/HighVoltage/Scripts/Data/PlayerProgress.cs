using System;

namespace HighVoltage.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public bool HasFinishedTutorial = false;
        public int CurrentLevel = 1;

        public override string ToString() 
            => $"Level={CurrentLevel};";
    } 
}
