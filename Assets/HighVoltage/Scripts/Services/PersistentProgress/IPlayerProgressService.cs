using HighVoltage.Infrastructure.Services;
using HighVoltage.Scripts.Data;

namespace HighVoltage.Services.Progress
{
    /// <summary>
    /// Service that provides access to player progress
    /// </summary>
    public interface IPlayerProgressService : IService
    {
        PlayerProgress Progress { get; set; }
        void IncrementCurrentLevel(bool objectiveReached);
    }
}