using System;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.Windows
{
    public class HubMenu : WindowBase
    {
        [SerializeField] private Button startGame;
        [SerializeField] private Button settings;

        public event EventHandler PlayerLaunchedTutorial = delegate { };
        public event EventHandler PlayerLaunchedGame = delegate { };

        private void Awake()
        {
            startGame.onClick.AddListener(LaunchGame);
            settings.onClick.AddListener(DisplaySettingsWindow);
        }

        private void DisplaySettingsWindow() 
            => WindowService.Open(WindowId.Settings);

        private void LaunchGame() 
            => PlayerLaunchedGame(null, null);

        public override void OnOpened() 
            => base.OnOpened();
    }
}