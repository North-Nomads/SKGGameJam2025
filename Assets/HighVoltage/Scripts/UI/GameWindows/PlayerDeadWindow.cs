using System;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.GameWindows;

namespace HighVoltage.UI.Windows
{
    public class PlayerDeadWindow : GameWindowBase
    {
        [SerializeField] private Button restartButton;

        public event EventHandler PlayerReturnedToMenu = delegate { };

        private void Awake() 
            => restartButton.onClick.AddListener(GoToMenu);

        private void GoToMenu() 
            => PlayerReturnedToMenu(null, null);
    }
}