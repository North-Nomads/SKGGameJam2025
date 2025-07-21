using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Level;
using HighVoltage.Map.Building;
using HighVoltage.Services;
using HighVoltage.Services.Progress;
using HighVoltage.StaticData;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace HighVoltage.Infrastructure.States
{
    public class LoadTutorialState : IState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ITutorialService _tutorialService;
        private readonly IUIFactory _uiFactory;
        private readonly Canvas _loadingCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly ICameraService _cameraService;
        private readonly IGameWindowService _gameWindowService;
        private readonly IPlayerBuildingService _buildingService;
        private readonly IBuildingStoreService _buildingStore;
        private readonly ILevelProgress _levelProgress;
        private readonly IPlayerProgressService _playerProgress;
        private readonly IGameFactory _gameFactory;
        private readonly IMobSpawnerService _mobSpawnerService;
        
        private TutorialWindow _currentTutorialWindow;
        private TutorialScenario _currentTutorialScenario;
        private InGameHUD _hudInstance;

        public LoadTutorialState(GameStateMachine gameStateMachine, IUIFactory uiFactory, Canvas loadingCurtain,
            IStaticDataService staticDataService, ITutorialService tutorialService, SceneLoader sceneLoader, 
            ICameraService cameraService, IGameWindowService gameWindowService, IPlayerBuildingService buildingService,
            IBuildingStoreService buildingStore, ILevelProgress levelProgress, IPlayerProgressService playerProgress,
            IGameFactory gameFactory, IMobSpawnerService mobSpawnerService)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
            _tutorialService = tutorialService;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _cameraService = cameraService;
            _gameWindowService = gameWindowService;
            _buildingService = buildingService;
            _buildingStore = buildingStore;
            _levelProgress = levelProgress;
            _playerProgress = playerProgress;
            _gameFactory = gameFactory;
            _mobSpawnerService = mobSpawnerService;
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.TutorialSceneName, onLoaded: OnLoaded);
            _tutorialService.UserRequiresNewTutorialStep -= OnUserRequiresNewTutorialStep;
        }

        private void OnLoaded()
        {
            LevelConfig config = _staticDataService.ForLevel(Constants.TutorialLevelIndex);
            PlayerCore playerCore = InitializeGameWorld(config);
            _levelProgress.LoadLevelConfig(config, playerCore);

            _buildingService.MapTilemap = Object.FindObjectOfType<Tilemap>(); //if it works
            _buildingService.OnSceneLoaded();
            InitializeBuilder();
            InitializeCamera();
            InitializeUI();
            _buildingStore.AddMoney(config.InitialMoney);
            _gameStateMachine.Enter<TutorialLoopState>();
        }
        
        private void InitializeBuilder()
        {
            PlayerBuildBehaviour playerBuildBehaviour = _gameFactory.CreateBuilder();
            playerBuildBehaviour.Initialize(_staticDataService, _buildingService, _gameWindowService);
        }
        
        private PlayerCore InitializeGameWorld(LevelConfig config)
        {
            PlayerCore playerCore = InitializePlayerBase();
            return playerCore;
            
            PlayerCore InitializePlayerBase()
            {
                PlayerCore playerCore = _gameFactory.CreatePlayerCore(GameObject.FindGameObjectWithTag(Constants.CoreSpawnPoint));
                playerCore.Initialize(_mobSpawnerService, config);
                return playerCore;
            }
        }

        private void InitializeUI()
        {
            _uiFactory.CreateUIRoot();
            InGameHUD inGameHUD = InitializeInGameHUD();
            InitializeTutorialWindow(inGameHUD);
            InitializePauseMenu();
        }
        
        private InGameHUD InitializeInGameHUD()
        {
            InGameHUD inGameHUD = _gameWindowService
                .GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>();
            
            inGameHUD.ProvideSceneData(_buildingService, _buildingStore);
            _gameWindowService.GetWindow(GameWindowId.EndGame);
            _gameWindowService.Open(GameWindowId.InGameHUD);
            inGameHUD.HideAllHUD();
            return inGameHUD;
        }
        
        private void InitializePauseMenu()
        {
            InGamePauseMenu pauseMenu = _gameWindowService.GetWindow(GameWindowId.InGamePauseMenu)
                .GetComponent<InGamePauseMenu>();
            pauseMenu.HideRestartButton();
            pauseMenu.RestartButtonPressed += (_, _) =>
            {
                _tutorialService.InterruptTutorial();
                _gameStateMachine.Enter<LoadTutorialState>();
            };
            pauseMenu.ReturnToMenuButtonPressed += (_, _) =>
            {
                _tutorialService.InterruptTutorial();
                _gameStateMachine.Enter<HubState>();
            };
        }

        private void InitializeCamera()
        {
            GameObject cameraSpawnPoint = GameObject.FindGameObjectWithTag(Constants.CameraSpawnPoint);
            _cameraService.InitializeCamera(cameraSpawnPoint.transform.position);
        }

        private void InitializeTutorialWindow(InGameHUD hudInstance)
        {
            _hudInstance = hudInstance;
            _currentTutorialWindow = _uiFactory.InstantiateTutorialMessageBox();
            _currentTutorialScenario = _staticDataService.GetTutorialScenario();
            foreach (TutorialMessage message in _currentTutorialScenario.TutorialMessages)
                _currentTutorialWindow.InitializeMessage(message);
            _tutorialService.UserRequiresNewTutorialStep += OnUserRequiresNewTutorialStep;
        }

        private void OnUserRequiresNewTutorialStep(object sender, TutorialMessage tutorialMessage)
        {
            _currentTutorialWindow.DisplayMessage(tutorialMessage);
            _hudInstance.EnableHUDPart(tutorialMessage.DisplayHUDPart);
            if (tutorialMessage.DisplayHUDPart == InGameHUDOptions.HUD)
                _buildingService.ToggleBuildingAllowance(true);
        }

        public void Exit()
        {
            _loadingCurtain.gameObject.SetActive(false);
        }
    }
}