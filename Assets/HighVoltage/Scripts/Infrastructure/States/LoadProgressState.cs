using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Scripts.Data;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPlayerProgressService _progressService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPlayerProgressService progressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<HubState>();
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress() 
            => new(1);
    }
}