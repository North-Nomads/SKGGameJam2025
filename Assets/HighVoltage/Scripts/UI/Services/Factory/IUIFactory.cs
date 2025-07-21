using System.Collections.Generic;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.PopUps;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;
using HighVoltage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        WindowBase InstantiateWindow(WindowId windowID);
        GameWindowBase InstantiateWindow(GameWindowId windowID);
        PopupWindow InstantiatePopupWindow(PopupWindowId popupWindowId);
        BuildingCard InstantiateBuildingCard(Transform buildingCardParent);
        List<LevelSelectButton> InstantiateLevelButtons(int totalLevels, Transform parent);
        List<Image> CreateTutorialImages(Sprite[] messageSprites, Transform parent);
        TutorialWindow InstantiateTutorialMessageBox();
    }
}