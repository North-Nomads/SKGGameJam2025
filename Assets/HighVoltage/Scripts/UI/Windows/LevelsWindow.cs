using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.Windows
{
    public class LevelsWindow : WindowBase
    {
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private Button exitButton;
        [SerializeField] private LeanTweenType displayAnimationType;
        [SerializeField] private float animationTime = 0.5f;

        public event EventHandler<int> LevelLaunched = delegate { };

        private void Awake()
        {
            exitButton.onClick.AddListener(() => WindowService.Open(WindowId.Hub));
        }

        public override void OnOpened()
        {
            base.OnOpened();
            AnimateDisplay();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            LeanTween
                .moveLocal(gameObject, new Vector3(Screen.width + Screen.width / 2, 0, 0), animationTime)
                .setEase(displayAnimationType)
                .setOnComplete(() => gameObject.SetActive(false));
        }

        private void AnimateDisplay()
        {
            RectTransform.localPosition = new Vector3(Screen.width, 0, 0);
            LeanTween.moveLocal(gameObject, Vector3.zero, animationTime).setEase(displayAnimationType);
        }

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