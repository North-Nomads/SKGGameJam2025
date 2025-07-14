using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using UnityEngine;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Level;
using HighVoltage.StaticData;
using System;
using HighVoltage.Map;
using HighVoltage.Services;

namespace HighVoltage.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IPlayerProgressService _progressService;
        private readonly IMobSpawnerService _mobSpawnerService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly Canvas _loadingCurtain;
        private readonly ITileGenerator _tileGenerator;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain, IGameFactory gameFactory, IPlayerProgressService progressService,
            IMobSpawnerService mobSpawnerService, IStaticDataService staticData, ITileGenerator tileGenerator)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _mobSpawnerService = mobSpawnerService;
            _staticData = staticData;
            _tileGenerator = tileGenerator;
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