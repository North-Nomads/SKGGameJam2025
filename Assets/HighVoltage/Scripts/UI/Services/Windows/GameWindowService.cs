using System.Collections.Generic;
using HighVoltage.Level;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;

namespace HighVoltage.UI.Services
{
    public class GameWindowService : IGameWindowService
    {
        private readonly Dictionary<GameWindowId, GameWindowBase> _windows;
        private readonly IUIFactory _uiFactory;
        private readonly ILevelProgress _levelProgress;


        private GameWindowId _previousWindow;
        private GameWindowId _currentWindow;

        public GameWindowService(IUIFactory uiFactory, ILevelProgress levelProgress)
        {
            _windows = new();
            _uiFactory = uiFactory;
            _levelProgress = levelProgress;
        }

        public void CleanUp()
        {
            _currentWindow = GameWindowId.InGameHUD;
            _previousWindow = GameWindowId.Unknown;
            _windows.Clear();
        }

        public GameWindowBase GetWindow(GameWindowId windowID)
        {
            _windows.TryGetValue(windowID, out var window);
            if (window == null)
            {
                window = _uiFactory.InstantiateWindow(windowID);
                window.ConstructWindow(this, _levelProgress);
                _windows[windowID] = window;
            }
            return window;
        }

        public void Open(GameWindowId windowId)
        {
            foreach (var windowKeyValuePair in _windows)
            {
                windowKeyValuePair.Value.gameObject.SetActive(false);
                windowKeyValuePair.Value.OnClosed();
            }

            var window = GetWindow(windowId);
            _previousWindow = _currentWindow;
            _currentWindow = windowId;

            window.OnOpened();
            window.gameObject.SetActive(true);
        }

        public void ReturnToPreviousWindow()
            => Open(_previousWindow);
    }
}