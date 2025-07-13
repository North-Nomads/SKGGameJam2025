using UnityEngine;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.PopUps
{
    public abstract class PopupWindow : MonoBehaviour
    {
        protected PopupWindowId PopupWindowID;
        protected IWindowService WindowService;

        public virtual void ConstructWindow(PopupWindowId popupWindowId, IWindowService windowService)
        {
            PopupWindowID = popupWindowId;
            WindowService = windowService;
        }
    }
}