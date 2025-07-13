using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.GameWindows;

namespace HighVoltage.UI.Windows
{
    public class EndGameWindow : GameWindowBase
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Button nextButton;
        public EventHandler ReturnToHubButtonPressed = delegate { };

        protected override void OnStart()
        {
            base.OnStart();
            nextButton.onClick.AddListener(() => ReturnToHubButtonPressed(this, null));
            header.text = LevelProgress.RewardGranted ? "С наградой" : "Без награды";
        }

        protected override void Initialize()
        {
            base.Initialize();
            header.text = "ИЗМЕНИТЬ!!";
        }
    }
}