using System;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Services.Windows;
using HighVoltage.UI.Windows;

namespace HighVoltage.StaticData
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public WindowBase Prefab;
    }

    [Serializable]
    public class GameWindowConfig
    {
        public GameWindowId WindowId;
        public GameWindowBase Prefab;
    }
}