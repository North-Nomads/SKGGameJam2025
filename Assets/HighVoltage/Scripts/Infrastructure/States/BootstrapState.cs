using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.HubVisuals;
using HighVoltage.Infrastructure.InGameTime;
using HighVoltage.Infrastructure.MobSpawnerService;
using HighVoltage.Infrastructure.ModelDisplayService;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Level;
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

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _allServices = services;
            RegisterServies();
        }

        public void Enter() 
            => EnterHub();

        public void Exit() {}

        private void EnterHub()
            => _gameStateMachine.Enter<LoadProgressState>();

        private void RegisterServies()
        {
            RegisterStaticDataService();
            _allServices.RegisterSingle<IHubVFX>(new HubVFX());
            _allServices.RegisterSingle<IInGameTimeService>(new InGameTimeService());

            _allServices.RegisterSingle<IInputService>(GetInputService());
            
            _allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            _allServices.RegisterSingle<IPlayerProgressService>(new PlayerProgressService());
            _allServices.RegisterSingle<IGameFactory>(new GameFactory(_allServices.Single<IAssetProvider>(),
                                                                      _allServices.Single<IPlayerProgressService>()));
            _allServices.RegisterSingle<IMobSpawnerService>(new MobSpawner(_allServices.Single<IGameFactory>()));
            _allServices.RegisterSingle<ILevelProgress>(new LevelProgress(_allServices.Single<IMobSpawnerService>()));
            _allServices.RegisterSingle<IModelDisplayService>(new InGameModelDisplayService(_allServices.Single<IGameFactory>(),
                                                                                            _allServices.Single<IPlayerProgressService>()));
            _allServices.RegisterSingle<ISaveLoadService>(new PlayerPrefsSaveLoadService(_allServices.Single<IPlayerProgressService>(),
                                                                                         _allServices.Single<IGameFactory>(),
                                                                                         _allServices.SaveWriterServices));
            _allServices.RegisterSingle<IUIFactory>(new UIFactory(_allServices.Single<IAssetProvider>(),
                                                                  _allServices.Single<IPlayerProgressService>(),
                                                                  _allServices.Single<IStaticDataService>()));
            _allServices.RegisterSingle<IGameWindowService>(new GameWindowService(_allServices.Single<IUIFactory>(),
                _allServices.Single<ILevelProgress>()));
            _allServices.RegisterSingle<ICameraService>(new CinemachineCameraService(_allServices.Single<IGameFactory>()));
            _allServices.RegisterSingle<IWindowService>(new WindowService(_allServices.Single<IUIFactory>(),
                                                                          _allServices.Single<IPlayerProgressService>(),
                                                                          _allServices.Single<ISaveLoadService>(),
                                                                          _allServices.Single<IGameFactory>(),
                                                                          _allServices.Single<ICameraService>(),
                                                                          _allServices.Single<IModelDisplayService>(),
                                                                          _allServices.Single<IHubVFX>()));
        }

        private void RegisterStaticDataService()
        {
            var staticData = new StaticDataService();
            staticData.LoadLevels();
            staticData.LoadWindows();
            staticData.LoadGameWindows();
            staticData.LoadLevelTasks();
            _allServices.RegisterSingle<IStaticDataService>(staticData);
        }

        private static IInputService GetInputService()
        {
            return new DesktopInputService();
            /*if (Application.isEditor)
            else
                return new MobileInputService();*/
        }
    }
}