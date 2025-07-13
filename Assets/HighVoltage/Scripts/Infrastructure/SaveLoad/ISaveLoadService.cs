using HighVoltage.Infrastructure.Services;
using HighVoltage.Scripts.Data;

namespace HighVoltage.Infrastructure.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}