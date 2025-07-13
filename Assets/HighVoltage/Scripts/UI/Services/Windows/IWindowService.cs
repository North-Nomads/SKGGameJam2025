using UnityEngine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.UI.PopUps;
using HighVoltage.UI.Windows;

namespace HighVoltage.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        void CleanUp();
        void Open(WindowId windowID, bool closePopup=false);
        WindowBase GetWindow(WindowId windowID);
        void ReturnToPreviousWindow();
        PopupWindow GetPopupWindow(PopupWindowId popupWindowId);
        GameObject OpenPopup(PopupWindowId popupWindowId);
        void CloseCurrentPopUp();
    }
}