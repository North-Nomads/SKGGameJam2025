using HighVoltage.HighVoltage.Scripts.UI.GameWindows;
using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
using HighVoltage.Services.Progress;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.GameWindows;
using TMPro;
using UnityEngine;

namespace HighVoltage.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoad;
        private readonly IPlayerProgressService _progressService;
        private readonly IInGameTimeService _timeService;
        private readonly IGameWindowService _gameWindowService;
        private readonly ILevelProgress _levelProgress;
        private readonly IMobSpawnerService _mobSpawnerService;
        private InGameHUD _inGameHUD;
        private bool _isWaveOngoing;

        public GameLoopState(GameStateMachine gameStateMachine, ISaveLoadService saveLoad,
            IGameWindowService gameWindowService, ILevelProgress levelProgress, IMobSpawnerService mobSpawnerService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoad = saveLoad;
            _gameWindowService = gameWindowService;
            _levelProgress = levelProgress;
            _mobSpawnerService = mobSpawnerService;
            _timeService = AllServices.Container.Single<IInGameTimeService>();
        }

        public void Enter()
        {
            _inGameHUD = _gameWindowService.GetWindow(GameWindowId.InGameHUD).GetComponent<InGameHUD>();
            BeforeGameHUD beforeGameHUD = _gameWindowService.GetWindow(GameWindowId.BeforeGameHUD).GetComponent<BeforeGameHUD>();
            beforeGameHUD.PlayerReadyToStart += (_, __) =>
            {
                StartGame();
            };

            _inGameHUD.NextWaveTimerIsUp += (_, __) =>
            {
                _mobSpawnerService.UpdateWaveOngoingStatus(true);
                _mobSpawnerService.LaunchMobSpawning();
            };
            _levelProgress.WaveCleared += (_, __) =>
            {
                _mobSpawnerService.UpdateWaveOngoingStatus(false);
                _mobSpawnerService.UpdateWaveContent(_levelProgress.LoadedWave);
                _inGameHUD.SetNextWaveTimer(_levelProgress.GetCurrentWaveTimer());
            };
            _levelProgress.LevelCleared += (_, __) =>
            {
                _mobSpawnerService.UpdateWaveOngoingStatus(false);
                _gameStateMachine.Enter<GameFinishedState>();
            };
            _levelProgress.PlayerCoreDestroyed += (_, __) =>
            {
                _gameStateMachine.Enter<GameFinishedState>();
            };
        }

        private void StartGame()
        {
            _inGameHUD.SetNextWaveTimer(_levelProgress.LoadedWave.SecondsDelayBeforeWave);
            _gameWindowService.Open(GameWindowId.InGameHUD);
        }

        public void Exit()
        {
            _timeService.RestoreTimePassage();
        }
    }
}