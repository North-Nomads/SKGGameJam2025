using HighVoltage.Infrastructure.Tutorial;

namespace HighVoltage.Infrastructure.States
{
    public class TutorialLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ITutorialService _tutorialService;

        public TutorialLoopState(GameStateMachine gameStateMachine, ITutorialService tutorialService)
        {
            _gameStateMachine = gameStateMachine;
            _tutorialService = tutorialService;
        }

        public void Enter()
        {
            _tutorialService.StartTutorial();
        }
        
        public void Exit()
        {
            
        }
    }
}