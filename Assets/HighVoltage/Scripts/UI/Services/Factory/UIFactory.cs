using UnityEngine;
using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Services.Progress;
using HighVoltage.StaticData;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;
using HighVoltage.UI.Windows;
using UnityEditor;
using PopupWindow = HighVoltage.UI.PopUps.PopupWindow;

namespace HighVoltage.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "Prefabs/UI/UIRoot";
        private const string CharacterButtonPath = "Prefabs/UI/HUB/Windows/CharacterCard_Button";

        private readonly IPlayerProgressService _playerProgress;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assets;
        private Transform _uiRoot;

        public UIFactory(IAssetProvider assets, IPlayerProgressService playerProgress, IStaticDataService staticData)
        {
            _assets = assets;
            _playerProgress = playerProgress;
            _staticData = staticData;
        }

        public void CreateUIRoot()
            => _uiRoot = _assets.Instantiate(UIRootPath).transform;

        public WindowBase InstantiateWindow(WindowId windowID)
        {
            var window = CreateSpecificWindow(windowID);
            window.transform.SetAsFirstSibling();
            window.gameObject.SetActive(false);
            return window;

            WindowBase CreateSpecificWindow(WindowId windowId)
            {
                return windowId switch
                {
                    WindowId.Hub => CreateHubMenu(),
                    WindowId.Settings => CreateSettingsMenu(),
                    _ => null,
                };
            }
        }

        private WindowBase CreateSettingsMenu()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Settings);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            return window;
        }

        public GameWindowBase InstantiateWindow(GameWindowId windowID)
        {
            var window = CreateSpecificWindow(windowID);
            window.transform.SetAsFirstSibling();
            window.gameObject.SetActive(false);
            return window;

            GameWindowBase CreateSpecificWindow(GameWindowId windowId)
            {
                return windowId switch
                {
                    GameWindowId.InGameHUD => CreateInGameHUD(),
                    GameWindowId.PlayerDead => CreatePlayerDeadWindow(),
                    _ => null,
                };
            }
        }

        private GameWindowBase CreatePlayerDeadWindow()
        {
            GameWindowConfig config = _staticData.ForGameWindow(GameWindowId.PlayerDead);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            return window;
        }


        private GameWindowBase CreateInGameHUD()
        {
            GameWindowConfig config = _staticData.ForGameWindow(GameWindowId.InGameHUD);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            return window;
        }

        public PopupWindow InstantiatePopupWindow(PopupWindowId popupWindowId)
        {
            var window = CreateSpecificPopupWindow(popupWindowId);
            window.transform.SetAsFirstSibling();
            return window;

            PopupWindow CreateSpecificPopupWindow(PopupWindowId windowId)
            {
                return windowId switch
                {
                    _ => null,
                };
            }
        }

        public BuildingCard InstantiateBuildingCard(Transform buildingCardParent) 
            => _assets.Instantiate<BuildingCard>(AssetPath.BuildingCard, buildingCardParent);

        private GameWindowBase CreateEndGame()
        {
            GameWindowConfig config = _staticData.ForGameWindow(GameWindowId.EndGame);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            return window;
        }

        private WindowBase CreateHubMenu()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Hub);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            return window;
        }
    }
}