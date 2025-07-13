using HighVoltage.Scripts.Data;

namespace HighVoltage.Services.Progress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
}