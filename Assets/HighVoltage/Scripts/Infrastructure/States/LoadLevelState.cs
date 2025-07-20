using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.CameraService;
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
        private readonly ILevelProgress _levelProgress;
        private readonly IBuildingStoreService _buildingStore;
        private readonly ICameraService _cameraService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IMobSpawnerService mobSpawnerService,
            IStaticDataService staticData, IGameWindowService gameWindowService, IUIFactory uiFactory,
            IPlayerBuildingService buildingService, ILevelProgress levelProgress, IBuildingStoreService buildingStore,
            ICameraService cameraService)
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
            _buildingStore = buildingStore;
            _cameraService = cameraService;
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
            _buildingStore.ResetMoney();
            InitializeMobSpawners();
            _buildingService.MapTilemap = Object.FindObjectOfType<Tilemap>(); //if it works
            _buildingService.OnSceneLoaded();
            InitializeBuilder();
            InitializeInGameHUD();
            InitializeCamera();
            _buildingStore.AddMoney(config.InitialMoney);
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitializeCamera()
        {
            GameObject cameraSpawnPoint = GameObject.FindGameObjectWithTag(Constants.CameraSpawnPoint);
            _cameraService.InitializeCamera(cameraSpawnPoint.transform.position);
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

        private void InitializeInGameHUD()
        {
            _uiFactory.CreateUIRoot();
            _gameWindowService.GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>()
                .ProvideSceneData(_buildingService, _buildingStore);
            _gameWindowService.GetWindow(GameWindowId.EndGame);
            _gameWindowService.GetWindow(GameWindowId.BeforeGameHUD);
            InitializePauseMenu();
            _gameWindowService.Open(GameWindowId.BeforeGameHUD);
        }

        private void InitializePauseMenu()
        {
            InGamePauseMenu pauseMenu = _gameWindowService.GetWindow(GameWindowId.InGamePauseMenu)
                .GetComponent<InGamePauseMenu>();
            pauseMenu.RestartButtonPressed += (_, _) =>
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