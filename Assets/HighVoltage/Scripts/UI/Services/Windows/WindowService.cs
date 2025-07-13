using System;
using System.Collections.Generic;
using UnityEngine;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services.Progress;
using HighVoltage.UI.PopUps;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Windows;

namespace HighVoltage.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private WindowId _previousWindow;
        private WindowId _currentWindow;
        private PopupWindowId _currentPopup;
        private readonly Dictionary<PopupWindowId, PopupWindow> _popupWindows;
        private readonly Dictionary<WindowId, WindowBase> _windows;

        private readonly IUIFactory _uiFactory;
        private readonly IPlayerProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameFactory _gameFactory;
        private readonly ICameraService _cameraService;

        public WindowService(IUIFactory uiFactory, IPlayerProgressService progressService,
            ISaveLoadService saveLoadService, IGameFactory gameFactory, ICameraService cameraService)
        {
            _uiFactory = uiFactory;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _windows = new Dictionary<WindowId, WindowBase>();
            _popupWindows = new Dictionary<PopupWindowId, PopupWindow>();
            _gameFactory = gameFactory;
            _cameraService = cameraService;
        }

        public void CleanUp()
        {
            _currentWindow = WindowId.Hub;
            _previousWindow = WindowId.Unknown;
            _windows.Clear();
        }

        public WindowBase GetWindow(WindowId windowID)
        {
            _windows.TryGetValue(windowID, out var window);
            if (window == null)
            {
                window = _uiFactory.InstantiateWindow(windowID);
                window.ConstructWindow(_progressService, windowID, this, _saveLoadService, _gameFactory);
                _windows[windowID] = window;
            }

            return window;
        }

        public void Open(WindowId windowID, bool closePopUp=false)
        {
            // Has active pop up window (full-screen windows are disabled)
            if (_currentPopup != PopupWindowId.Unknown)
                return;

            var window = GetWindow(windowID);

            // Hide all windows except hub and target window
            foreach (var windowPair in _windows)
            {
                if (windowPair.Key != windowID)    
                    windowPair.Value.gameObject.SetActive(false);
            }

            _previousWindow = _currentWindow;
            _currentWindow = windowID;

            window.OnOpened();
            _cameraService.MoveCamera(windowID);
            window.gameObject.SetActive(true);
        }

        public void ReturnToPreviousWindow() 
            => Open(_previousWindow);

        private void OpenHubMenu(object sender, EventArgs e) 
            => Open(WindowId.Hub);

        public PopupWindow GetPopupWindow(PopupWindowId popupWindowId)
        {
            _popupWindows.TryGetValue(popupWindowId, out var window);
            if (window == null)
            {
                window = _uiFactory.InstantiatePopupWindow(popupWindowId);
                window.ConstructWindow(popupWindowId, this);
                _popupWindows[popupWindowId] = window;
                window.gameObject.SetActive(false);
            }
            return window;
        }

        public GameObject OpenPopup(PopupWindowId popupWindowId)
        {
            var window = GetPopupWindow(popupWindowId);

            // Hide all windows except hub and target window
            foreach (var popupPair in _popupWindows)
                popupPair.Value.gameObject.SetActive(false);

            window.gameObject.SetActive(true);
            _currentPopup = popupWindowId;
            return window.gameObject;
        }

        public void CloseCurrentPopUp()
        {
            if (_currentPopup == PopupWindowId.Unknown)
                return;

            GetPopupWindow(_currentPopup).gameObject.SetActive(false);
            _currentPopup = PopupWindowId.Unknown;
        }
    }
}