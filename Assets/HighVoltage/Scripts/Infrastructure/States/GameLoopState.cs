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
            InGameHUD gameWindowBase = _gameWindowService.GetWindow(GameWindowId.InGameHUD).GetComponent<InGameHUD>();
            gameWindowBase.SetNextWaveTimer(_levelProgress.LoadedWave.SecondsDelayBeforeWave);

            gameWindowBase.NextWaveTimerIsUp += (_, __) => _mobSpawnerService.LaunchMobSpawning();
            _levelProgress.WaveCleared += (_, __) =>
            {
                _mobSpawnerService.UpdateWaveContent(_levelProgress.LoadedWave);
                gameWindowBase.SetNextWaveTimer(_levelProgress.GetCurrentWaveTimer());
            };
            _levelProgress.LevelCleared += (_, __) => Debug.Log("Level cleared! Yuppy");
        }

        public void Exit()
        {
            _timeService.RestoreTimePassage();
        }
    }
}