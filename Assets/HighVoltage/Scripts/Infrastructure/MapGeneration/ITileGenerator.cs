using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Map
{
    public interface ITileGenerator : IService
    {
        void LoadAndGenerateMap(int levelNumber);
    }
}