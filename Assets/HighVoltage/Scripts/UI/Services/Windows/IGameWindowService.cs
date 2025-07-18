using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Infrastructure.Services;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services.GameWindows;
using UnityEngine;

namespace HighVoltage.UI.Services
{
    public interface IGameWindowService : IService
    {
        void CleanUp();
        bool HasOpenedWindows();
        GameWindowBase GetWindow(GameWindowId endGame);
        void Open(GameWindowId windowId);
        void ReturnToPreviousWindow();
        BuildingCard CreateBuildingCard(BuildingConfig building, Transform parent);
    }
}