using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoad;
        private readonly IPlayerProgressService _progressService;
        private readonly IInGameTimeService _timeService;

        public GameLoopState(GameStateMachine gameStateMachine, ISaveLoadService saveLoad)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoad = saveLoad;
            _timeService = AllServices.Container.Single<IInGameTimeService>();
        }

        public void Enter()
        { }

        public void Exit()
        {
            _timeService.RestoreTimePassage();
        }
    }
}