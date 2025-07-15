using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.Windows;
using UnityEngine;

namespace HighVoltage.UI.Windows
{
    public class LevelsWindow : WindowBase
    {
        [SerializeField] private Transform buttonsParent;

        public event EventHandler<int> LevelLaunched = delegate { };

        public override void ConstructWindow(IPlayerProgressService progressService, WindowId windowId, IWindowService windowService,
            ISaveLoadService saveLoadService, IGameFactory gameFactory, IUIFactory uiFactory)
        {
            base.ConstructWindow(progressService, windowId, windowService, saveLoadService, gameFactory, uiFactory);
            List<LevelSelectButton> buttons = UIFactory.InstantiateLevelButtons(Constants.TotalLevels, buttonsParent);
            foreach (LevelSelectButton button in buttons) 
                button.LevelButtonPressed += (_, levelIndex) => LevelLaunched(this, levelIndex);
        }
    }
    
}