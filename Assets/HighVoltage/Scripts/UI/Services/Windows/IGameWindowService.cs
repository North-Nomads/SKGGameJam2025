using HighVoltage.Infrastructure.Services;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services.GameWindows;

namespace HighVoltage.UI.Services
{
    public interface IGameWindowService : IService
    {
        void CleanUp();
        GameWindowBase GetWindow(GameWindowId endGame);
        void Open(GameWindowId windowId);
        void ReturnToPreviousWindow();
    }
}