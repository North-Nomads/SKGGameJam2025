using System;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Services.Windows;
using TMPro;

namespace HighVoltage.UI.Windows
{
    public class HubMenu : WindowBase
    {
        [SerializeField] private TextMeshProUGUI startGameButtonText;
        [SerializeField] private Button startGame;
        [SerializeField] private Button allLevels;
        [SerializeField] private Button exit;

        public event EventHandler PlayerLaunchedGame = delegate { };

        private void Awake()
        {
            startGame.onClick.AddListener(LaunchGame);
            allLevels.onClick.AddListener(DisplayAllLevelsWindow);
            exit.onClick.AddListener(CloseGame);
        }

        public override void ConstructWindow(IPlayerProgressService progressService, WindowId windowId, IWindowService windowService,
            ISaveLoadService saveLoadService, IGameFactory gameFactory, IUIFactory uiFactory)
        {
            base.ConstructWindow(progressService, windowId, windowService, saveLoadService, gameFactory, uiFactory);
            if (!PlayerProgress.Progress.HasFinishedTutorial)
                startGameButtonText.text = "START TUTORIAL";
        }

        private void DisplayAllLevelsWindow() 
            => WindowService.Open(WindowId.Levels);

        private void CloseGame() 
            => Application.Quit();

        private void LaunchGame() 
            => PlayerLaunchedGame(null, null);
    }
}