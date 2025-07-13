using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawnerService;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.Level;
using HighVoltage.StaticData;
using HighVoltage.UI.GameWindows;
using System;
using HighVoltage.Map;

namespace HighVoltage.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerSpawnPointTag = "PlayerSpawnPoint";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly Canvas _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerProgressService _progressService;
        private readonly IUIFactory _uiFactory;
        private readonly IMobSpawnerService _mobSpawner;
        private readonly ILevelProgress _levelProgress;
        private readonly IStaticDataService _staticData;
        private readonly ITileGenerator _tileGenerator;
        private InGameHUD _hud;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IUIFactory uiFactory,
            IMobSpawnerService mobSpawner, ILevelProgress levelProgress, IStaticDataService staticData, ITileGenerator tileGenerator)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uiFactory = uiFactory;
            _mobSpawner = mobSpawner;
            _levelProgress = levelProgress;
            _staticData = staticData;
            _tileGenerator = tileGenerator;

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
            InitializeGameWorld();
            InstantiateUI();
            InitializeLevelTask();
            InformProgressReaders();
            GenerateWorldMap();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void GenerateWorldMap()
        {
            _tileGenerator.LoadAndGenerateMap($"Maps/{_progressService.Progress.CurrentLevel}.chertanovo");
        }

        private void InitializeLevelTask()
        {
            LevelTaskConfig selectedTask = _staticData.GetRandomLevelTask();
            _levelProgress.UpdateOnNewLevel(selectedTask);
            _hud.UpdateTask(selectedTask, _progressService.Progress.RemainingTasks);
        }

        private void InstantiateUI()
        {
            _uiFactory.CreateUIRoot();
            _hud = _uiFactory.InstantiateWindow(GameWindowId.InGameHUD).GetComponent<InGameHUD>();
            _hud.gameObject.SetActive(true);
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void InitializeGameWorld()
        {
            var playerInstance = _gameFactory.CreateHero(GameObject.FindGameObjectWithTag(PlayerSpawnPointTag));
            var virtualCamera = _gameFactory.CreateCamera(playerInstance);
            virtualCamera.Follow = playerInstance.transform;
            _mobSpawner.SpawnMobs(playerInstance);
        }
    }
}