using HighVoltage.Infrastructure.Services;
using HighVoltage.Data;

namespace HighVoltage.Infrastructure.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}