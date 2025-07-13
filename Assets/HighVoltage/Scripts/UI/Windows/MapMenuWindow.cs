using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Windows;

namespace HighVoltage.Scripts.UI.Windows
{
    public class MapMenuWindow : WindowBase
    {
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button exitButton;

        [Header("Footer")]
        [SerializeField] private Button playButton;

        public EventHandler LaunchGameButtonPressed = delegate { };

        private void Awake()
        {
            exitButton.onClick.AddListener(CloseWindow);
            playButton.onClick.AddListener(LaunchGame);
        }

        private void LaunchGame() 
            => LaunchGameButtonPressed(this, null);

        private void Start()
            => titleText.text = "УРОВЕНЬ: " + PlayerProgress.Progress.CurrentLevel.ToString();
    }
}
