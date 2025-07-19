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
        [SerializeField] private LeanTweenType displayAnimationType;
        [SerializeField] private float animationTime = 0.5f;
        

        public event EventHandler PlayerLaunchedGame = delegate { };

        private void Awake()
        {
            startGame.onClick.AddListener(LaunchGame);
            allLevels.onClick.AddListener(DisplayAllLevelsWindow);
            exit.onClick.AddListener(CloseGame);
        }

        protected override void OnStart()
        {
            base.OnStart();
            AnimateDisplay();
        }

        public override void OnOpened()
        {
            base.OnOpened();
            AnimateDisplay();
            Debug.Break();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            LeanTween
                .moveLocal(gameObject, new Vector3(-(Screen.width + Screen.width / 2), 0, 0), animationTime)
                .setEase(displayAnimationType)
                .setOnComplete(() => gameObject.SetActive(false));
        }

        private void AnimateDisplay()
        {
            RectTransform.localPosition = new Vector3(-Screen.width, 0, 0);
            LeanTween.moveLocal(gameObject, Vector3.zero, animationTime).setEase(displayAnimationType);
        }

        private void DisplayAllLevelsWindow() 
            => WindowService.Open(WindowId.Levels);

        private void CloseGame() 
            => Application.Quit();

        private void LaunchGame() 
            => PlayerLaunchedGame(null, null);
    }
}