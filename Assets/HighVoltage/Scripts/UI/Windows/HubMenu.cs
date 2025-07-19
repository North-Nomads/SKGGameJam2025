using System;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.Windows
{
    public class HubMenu : WindowBase
    {
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

        private void DisplayAllLevelsWindow() 
            => WindowService.Open(WindowId.Levels);

        private void CloseGame() 
            => Application.Quit();

        private void LaunchGame() 
            => PlayerLaunchedGame(null, null);
    }
}