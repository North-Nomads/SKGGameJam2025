using UnityEngine;
using UnityEngine.UI;
using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
using HighVoltage.UI.Services;

namespace HighVoltage.UI.GameWindows
{
    public class InGamePauseMenu : GameWindowBase
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;
        private IInGameTimeService _timeService;

        private void Awake()
        {
            continueButton.onClick.AddListener(ResumeGame);
            exitButton.onClick.AddListener(ExitToMenu);
        }

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

        private void ExitToMenu()
        {
        }

        private void ResumeGame()
        {
            GameWindowService.ReturnToPreviousWindow();
        }
    }
}