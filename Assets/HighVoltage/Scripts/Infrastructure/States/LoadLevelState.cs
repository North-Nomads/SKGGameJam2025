using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using System.Linq;
using HighVoltage.Services;
using HighVoltage.Map.Building;
using UnityEngine.Tilemaps;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Windows;
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
        private readonly ILevelProgress _levelProgress;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IMobSpawnerService mobSpawnerService,
            IStaticDataService staticData, IGameWindowService gameWindowService, IUIFactory uiFactory,
            IPlayerBuildingService buildingService, ILevelProgress levelProgress)
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
            _levelProgress = levelProgress;
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
            _levelProgress.LoadLevelConfig(config, playerCore);
            InitializeMobSpawners();
            _buildingService.MapTilemap = Object.FindObjectOfType<Tilemap>(); //if it works
            InitializeBuilder();
            InitializeInGameHUD(playerCore);
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
            return playerCore;
        }

        private void InitializeInGameHUD(PlayerCore playerCore)
        {
            _uiFactory.CreateUIRoot();
            
            _gameWindowService.GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>()
                .ProvideSceneData(playerCore, _buildingService);
            _gameWindowService.GetWindow(GameWindowId.EndGame);
            InitializePauseMenu();
            _gameWindowService.Open(GameWindowId.InGameHUD);
        }

        private void InitializePauseMenu()
        {
            InGamePauseMenu pauseMenu = _gameWindowService.GetWindow(GameWindowId.InGamePauseMenu)
                .GetComponent<InGamePauseMenu>();
            pauseMenu.ReloadButtonPressed += (_, _) =>
                _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
            pauseMenu.ReturnToMenuButtonPressed += (_, _) => _gameStateMachine.Enter<HubState>();
        }

        private PlayerCore InitializePlayerBase(LevelConfig config)
        {
            PlayerCore playerCore = _gameFactory.CreatePlayerCore(GameObject.FindGameObjectWithTag(Constants.CoreSpawnPoint));
            
            playerCore.Initialize(_mobSpawnerService, config);
            return playerCore;
        }


        private void InitializeMobSpawners()
        {
            WaypointHolder[] spawnerSpots = Object.FindObjectsByType<WaypointHolder>(FindObjectsSortMode.None);
            
            _mobSpawnerService.LoadConfigToSpawners(_levelProgress.LoadedWave, spawnerSpots,
                _levelProgress.LoadedLevelConfig.DeltaBetweenSpawns);
        }
    }
}