using System;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.GameWindows;

namespace HighVoltage.Infrastructure.States
{
    public class TutorialLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ITutorialService _tutorialService;
        private readonly IGameWindowService _gameWindowService;
        private readonly IBuildingStoreService _buildingStore;

        public TutorialLoopState(GameStateMachine gameStateMachine, ITutorialService tutorialService,
            IGameWindowService gameWindowService, IBuildingStoreService buildingStore)
        {
            _gameStateMachine = gameStateMachine;
            _tutorialService = tutorialService;
            _gameWindowService = gameWindowService;
            _buildingStore = buildingStore;
        }

        public void Enter()
        {
            _tutorialService.StartTutorial();
            _tutorialService.AllTutorialStepsFinished += HandleUserFinishedTutorial;
        }

        private void HandleUserFinishedTutorial(object sender, EventArgs e)
        {
            _gameStateMachine.Enter<LoadLevelState, string>("Level1");
        }

        public void Exit()
        {
            _gameWindowService
                .GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>()
                .OnLevelCompleted(_buildingStore);
        }
    }
}