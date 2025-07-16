using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Services;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Windows;
using UnityEngine.SceneManagement;

namespace HighVoltage.Infrastructure.States
{
    public class GameFinishedState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerProgressService _progressService;
        private readonly IGameWindowService _gameWindowService;
        private readonly IPlayerProgressService _playerProgress;
        private readonly IInGameTimeService _timeService;

        public GameFinishedState(GameStateMachine gameStateMachine, IPlayerProgressService progressService,
            IGameWindowService gameWindowService, IPlayerProgressService playerProgress, IInGameTimeService timeService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _gameWindowService = gameWindowService;
            _playerProgress = playerProgress;
            _timeService = timeService;
        }

        public void Enter()
        {
            EndGameWindow endGameWindow = _gameWindowService.GetWindow(GameWindowId.EndGame).GetComponent<EndGameWindow>();
            _gameWindowService.Open(GameWindowId.EndGame);
            _timeService.EnablePause();
            endGameWindow.ReturnToHubButtonPressed += (_, __) =>
            {
                _gameStateMachine.Enter<LoadProgressState>();
            };
            endGameWindow.RestartLevelButtonPressed += (_, __) =>
            {
                _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
            };
            endGameWindow.LaunchNextLevelButtonPressed += (_, __) =>
            {
                _progressService.IncrementCurrentLevel();
                if (_playerProgress.Progress.CurrentLevel >= Constants.TotalLevels)
                    _gameStateMachine.Enter<HubState>();
                else
                    _gameStateMachine.Enter<LoadLevelState, string>($"Level{_playerProgress.Progress.CurrentLevel}");
            };
        }

        public void Exit() 
        {
            _timeService.RestoreTimePassage();
        }
    }
}