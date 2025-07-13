using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.ModelDisplayService
{
    public interface IModelDisplayService : IService
    {
        void OnHubLoading();
    } 
}