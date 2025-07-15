using HighVoltage.Data;

namespace HighVoltage.Services.Progress
{
    public interface IProgressUpdater : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress progress);
    }
}