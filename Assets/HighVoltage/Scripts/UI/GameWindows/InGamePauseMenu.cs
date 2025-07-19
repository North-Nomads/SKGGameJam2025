using System;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.GameWindows;
using UnityEngine.InputSystem;

namespace HighVoltage.UI.GameWindows
{
    public class InGamePauseMenu : GameWindowBase
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button restartButton; 
        private IInGameTimeService _timeService;
        private PlayerInput _inputActions;

        public event EventHandler ReloadButtonPressed = delegate { };
        public event EventHandler ReturnToMenuButtonPressed = delegate { };
        

        private void Awake()
        {
            // these two buttons do the same
            continueButton.onClick.AddListener(ResumeGame);
            
            restartButton.onClick.AddListener(() => ReloadButtonPressed(this, EventArgs.Empty));
            restartButton.onClick.AddListener(() => Debug.Log("Restart game"));
            exitButton.onClick.AddListener(() => ReturnToMenuButtonPressed(this, EventArgs.Empty));
            _inputActions = new PlayerInput();
            _inputActions.Enable();
            _inputActions.Editing.OpenPauseMenu.performed += OpenPauseMenu;
        }

        private void OpenPauseMenu(InputAction.CallbackContext obj)
            => GameWindowService.Open(GameWindowId.InGamePauseMenu);

        public override void ConstructWindow(IGameWindowService gameWindowService, ILevelProgress levelProgress)
        {
            base.ConstructWindow(gameWindowService, levelProgress);
            _timeService = AllServices.Container.Single<IInGameTimeService>();
        }

        public override void OnOpened()
        {
            base.OnOpened();
            _timeService.EnablePause();
        }

        public override void OnClosed()
        {
            base.OnClosed();
            _timeService.RestoreTimePassage();
        }

        private void ResumeGame()
        {
            GameWindowService.ReturnToPreviousWindow();
        }
    }
}