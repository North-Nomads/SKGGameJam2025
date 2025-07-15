using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using System.Collections.Generic;
using System.Linq;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Services;
using HighVoltage.Map.Building;
using UnityEngine.Tilemaps;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace HighVoltage.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IPlayerProgressService _progressService;
        private readonly IMobSpawnerService _mobSpawnerService;
        private readonly IGameWindowService _gameWindowService;
        private readonly IUIFactory _uiFactory;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly Canvas _loadingCurtain;
        private readonly IPlayerBuildingService _buildingService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IMobSpawnerService mobSpawnerService,
            IStaticDataService staticData, IGameWindowService gameWindowService, IUIFactory uiFactory,
            IPlayerBuildingService buildingService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _mobSpawnerService = mobSpawnerService;
            _staticData = staticData;
            _gameWindowService = gameWindowService;
            _uiFactory = uiFactory;
            _buildingService = buildingService;
        }

        private void OnLevelFinished(object sender, bool shouldGiveReward)
        {
            if (_progressService.Progress.IsLastLevel)
            {
                _gameStateMachine.Enter<HubState>();
                return;
            }
            _progressService.IncrementCurrentLevel(shouldGiveReward);
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
            LevelConfig config = _staticData.ForLevel(_progressService.Progress.CurrentLevel);
            PlayerCore playerCore = InitializeGameWorld(config);
            List<SentryConfig> thisLevelSentries = config.SentryIDs
                .Select(sentryID => _staticData.ForSentryID(sentryID)).ToList();

            _buildingService.MapTilemap = Object.FindObjectOfType<Tilemap>(); //if it works
            InitializeBuilder();
            InitializeInGameHUD(playerCore, thisLevelSentries);
            _gameStateMachine.Enter<GameLoopState>();
        }
        
        private void InitializeBuilder()
        {
            PlayerBuildBehaviour playerBuildBehaviour = _gameFactory.CreateBuilder();
            playerBuildBehaviour.Initialize(_staticData, _buildingService, _gameWindowService);
        }

        private PlayerCore InitializeGameWorld(LevelConfig config)
        {
            PlayerCore playerCore = InitializePlayerBase(config);
            InitializeMobSpawners(config);
            return playerCore;
        }

        private void InitializeInGameHUD(PlayerCore playerCore, List<SentryConfig> thisLevelSentries)
        {
            _uiFactory.CreateUIRoot();
            
            _gameWindowService.GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>()
                .ProvideSceneData(playerCore, thisLevelSentries, _buildingService);
            
            InitializePauseMenu();

            _gameWindowService.Open(GameWindowId.InGameHUD);
        }

        private void InitializePauseMenu()
        {
            InGamePauseMenu pauseMenu = _gameWindowService.GetWindow(GameWindowId.InGamePauseMenu)
                .GetComponent<InGamePauseMenu>();
            pauseMenu.ReloadButtonPressed += (sender, args) =>
                _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
            pauseMenu.ReturnToMenuButtonPressed += (sender, args) => _gameStateMachine.Enter<HubState>();
        }

        private PlayerCore InitializePlayerBase(LevelConfig config)
        {
            PlayerCore playerCore = _gameFactory.CreatePlayerCore(GameObject.FindGameObjectWithTag(Constants.CoreSpawnPoint));
            
            playerCore.Initialize(_mobSpawnerService, config);
            return playerCore;
        }


        private void InitializeMobSpawners(LevelConfig config)
        {
            WaypointHolder[] spawnerSpots = Object.FindObjectsByType<WaypointHolder>(FindObjectsSortMode.None);
            
            if (spawnerSpots.Length != config.Gates.Length)
            {
                Debug.LogError("Gates number and level config gates number must be same. " + 
                                $"Spawner spots count: {spawnerSpots.Length} & Config gates: {config.Gates.Length}");
                return;
            }
            _mobSpawnerService.LoadConfigToSpawners(config, spawnerSpots);
        }
    }
}