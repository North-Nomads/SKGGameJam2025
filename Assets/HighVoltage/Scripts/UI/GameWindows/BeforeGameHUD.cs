using System;
using HighVoltage.UI.GameWindows;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.HighVoltage.Scripts.UI.GameWindows
{
    public class BeforeGameHUD : GameWindowBase
    {
        [SerializeField] private Button readyButton;

        public event EventHandler PlayerReadyToStart = delegate { };

        private void Awake()
        {
            readyButton.onClick.AddListener(() => PlayerReadyToStart(this, EventArgs.Empty));
        }
    }
}