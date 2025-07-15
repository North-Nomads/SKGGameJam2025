using HighVoltage.Data;

namespace HighVoltage.Services.Progress
{
    public class PlayerProgressService : IPlayerProgressService
    {
        public PlayerProgress Progress { get; set; }

        public void IncrementCurrentLevel(bool objectiveReached)
        {
            Progress.CurrentLevel++;
            if (objectiveReached)
            {
                Progress.RemainingTasks--;
            }
        }
    }
}