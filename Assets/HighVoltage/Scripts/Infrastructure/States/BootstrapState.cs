using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Level;
using HighVoltage.Map.Building;
using HighVoltage.Services.Inputs;
using HighVoltage.Services.Progress;
using HighVoltage.StaticData;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services, 
            ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _allServices = services;
            _coroutineRunner = coroutineRunner;
            RegisterServices();
        }

        public void Enter() 
            => EnterHub();

        public void Exit() {}

        private void EnterHub()
            => _gameStateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            RegisterStaticDataService();
            _allServices.RegisterSingle<IInGameTimeService>(new InGameTimeService());
            _allServices.RegisterSingle<IBuildingStoreService>(new BuildingStoreService());
            _allServices.RegisterSingle<IInputService>(new DesktopInputService());
            _allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            _allServices.RegisterSingle<IPlayerProgressService>(new PlayerProgressService());

            _allServices.RegisterSingle<IEventSenderService>(new EventSenderService());
            _allServices.RegisterSingle<IGameFactory>(new GameFactory(
                _allServices.Single<IAssetProvider>(),
                _allServices.Single<IPlayerProgressService>()));
            _allServices.RegisterSingle<IMobSpawnerService>(new MobSpawnerService(
                _allServices.Single<IGameFactory>(),
                _allServices.Single<IStaticDataService>()));
            _allServices.RegisterSingle<ILevelProgress>(new LevelProgress(
                _allServices.Single<IMobSpawnerService>(),
                _allServices.Single<IStaticDataService>(),
                _allServices.Single<IBuildingStoreService>()));
            _allServices.RegisterSingle<ISaveLoadService>(new PlayerPrefsSaveLoadService(
                _allServices.Single<IPlayerProgressService>(),
                _allServices.Single<IGameFactory>(),
                _allServices.SaveWriterServices));
            _allServices.RegisterSingle<IUIFactory>(new UIFactory(_allServices.Single<IAssetProvider>(),
                _allServices.Single<IPlayerProgressService>(),
                _allServices.Single<IStaticDataService>()));
            _allServices.RegisterSingle<IGameWindowService>(new GameWindowService(_allServices.Single<IUIFactory>(),
                _allServices.Single<ILevelProgress>()));
            _allServices.RegisterSingle<ICameraService>(
                new CinemachineCameraService(_allServices.Single<IGameFactory>()));
            _allServices.RegisterSingle<IWindowService>(new WindowService(_allServices.Single<IUIFactory>(),
                _allServices.Single<IPlayerProgressService>(),
                _allServices.Single<ISaveLoadService>(),
                _allServices.Single<IGameFactory>(),
                _allServices.Single<ICameraService>()));
            _allServices.RegisterSingle<IPlayerBuildingService>(new PlayerBuildingService(
                _allServices.Single<IStaticDataService>(),
                _allServices.Single<IGameFactory>(),
                _allServices.Single<IMobSpawnerService>(),
                _allServices.Single<IBuildingStoreService>(),
                _allServices.Single<IEventSenderService>()));
            _allServices.RegisterSingle<ITutorialService>(new TutorialService(
                _allServices.Single<IStaticDataService>(),
                _allServices.Single<IEventSenderService>(),
                _coroutineRunner,
                _allServices.Single<IPlayerProgressService>(),
                _allServices.Single<IPlayerBuildingService>()));
        }

        private void RegisterStaticDataService()
        {
            var staticData = new StaticDataService();
            staticData.LoadLevels();
            staticData.LoadTileAtlas();
            staticData.LoadEnemies();
            staticData.LoadWindows();
            staticData.LoadGameWindows();
            staticData.LoadSentries();
            staticData.LoadBuildingConfigs();
            staticData.LoadBuildingPrefabs();
            staticData.LoadWirePrefab();
            staticData.LoadTutorialScenario();
            _allServices.RegisterSingle<IStaticDataService>(staticData);
        }
    }
}