using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using HighVoltage.UI.GameWindows;
using HighVoltage.Services;

namespace HighVoltage.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IPlayerProgressService _progressService;
        private readonly IMobSpawnerService _mobSpawnerService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticData;
        private readonly ILevelProgress _levelProgress;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly Canvas _loadingCurtain;
        private InGameHUD _hud;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IMobSpawnerService mobSpawnerService,
            ILevelProgress levelProgress, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _mobSpawnerService = mobSpawnerService;
            _levelProgress = levelProgress;
            _staticData = staticData;

            _levelProgress.LevelFinishedWithReward += OnLevelFinished;
        }

        private void OnLevelFinished(object sender, bool shouldGiveReward)
        {
            if (_progressService.Progress.IsLastLevel)
            {
                _gameStateMachine.Enter<HubState>();
                return;
            }
            _progressService.IncrementCurrentLevel(shouldGiveReward);
            _hud.UpdateOnLevelFinished(shouldGiveReward, _progressService.Progress.RemainingTasks);
            _gameStateMachine.Enter<GameFinishedState>();
        }

        public void Enter(string sceneName)
        {
            _gameFactory.CleanUp();
            _loadingCurtain.gameObject.SetActive(true);
            _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.gameObject.SetActive(false);
        }

        private void OnLoaded()
        {
            GameObject playerCore = InitializePlayerBase();
            InitializeMobSpawners(playerCore);
            _gameStateMachine.Enter<GameLoopState>();
        }

        private GameObject InitializePlayerBase() 
            => _gameFactory.CreatePlayerCore(GameObject.FindGameObjectWithTag(Constants.CoreSpawnPoint));


        private void InitializeMobSpawners(GameObject playerCore)
        {
            GameObject[] spawnerSpots = GameObject.FindGameObjectsWithTag(Constants.MobSpawnerTag);
            LevelConfig config = _staticData.ForLevel(_progressService.Progress.CurrentLevel);
            _mobSpawnerService.LoadConfigToSpawners(config, spawnerSpots, playerCore);
        }
    }
}