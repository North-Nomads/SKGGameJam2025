using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using System;
using HighVoltage.HighVoltage.Scripts.Sentry;
using HighVoltage.Services;
using HighVoltage.UI.GameWindows;
using HighVoltage.UI.Services;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;
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

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IMobSpawnerService mobSpawnerService,
            IStaticDataService staticData, IGameWindowService gameWindowService, IUIFactory uiFactory)
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
            var playerCore = InitializeGameWorld();
            InitializeInGameHUD(playerCore);
            _gameStateMachine.Enter<GameLoopState>();
        }

        private PlayerCore InitializeGameWorld()
        {
            LevelConfig config = _staticData.ForLevel(_progressService.Progress.CurrentLevel);
            PlayerCore playerCore = InitializePlayerBase(config);
            InitializeMobSpawners(config);
            DEBUG_InitializeSentry();
            return playerCore;
        }

        private void DEBUG_InitializeSentry()
        {
            const int DEBUG_SentryID = 2;
            SentryConfig config = _staticData.ForSentryID(DEBUG_SentryID);
            SentryTower sentry = _gameFactory.CreateSentry(GameObject.FindGameObjectWithTag(Constants.DEBUG_SentrySpawn));
            sentry.Initialize(config, _mobSpawnerService, _gameFactory);
        }

        private void InitializeInGameHUD(PlayerCore playerCore)
        {
            _uiFactory.CreateUIRoot();
            
            _gameWindowService.GetWindow(GameWindowId.InGameHUD)
                .GetComponent<InGameHUD>()
                .ProvidePlayerCore(playerCore);
            _gameWindowService.Open(GameWindowId.InGameHUD);
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