using HighVoltage.Infrastructure.AssetManagement;
using HighVoltage.Infrastructure.CameraService;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.Windows;
using System.Collections.Generic;
using UnityEngine;
using HighVoltage.UI.Windows;
using System;
using HighVoltage.Services;
using HighVoltage.UI.Elements;
using UnityEngine.SceneManagement;

namespace HighVoltage.Infrastructure.States
{
    public class HubState : IState
    {
        private readonly List<ISavedProgressReader> _saveReaderServices;
        private readonly IPlayerProgressService _progressService;
        private readonly GameStateMachine _stateMachine;
        private readonly IWindowService _windowService;
        private readonly ICameraService _cameraService;
        private readonly IAssetProvider _assetProvider;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly Canvas _loadingCurtain;
        private readonly IUIFactory _uiFactory;

        public HubState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, Canvas loadingCurtain,
            IGameFactory gameFactory, IPlayerProgressService progressService, IUIFactory uiFactory, 
            IWindowService windowService, List<ISavedProgressReader> saveReaderServices, ICameraService cameraService,
            IAssetProvider assetProvider)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _saveReaderServices = saveReaderServices;
            _cameraService = cameraService;
            _assetProvider = assetProvider;
        }

        public void Enter()
        {
            _windowService.CleanUp();
            _sceneLoader.Load(Constants.HubSceneName, onLoaded: InitializeScene);
            _loadingCurtain.gameObject.SetActive(false);
        }

        public void Exit()
        {
            
        }


        private void InitializeScene()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            _uiFactory.CreateUIRoot();
            InitializeMenuWindows();
        }

        private void InformProgressReaders()
        {
            foreach (var saveService in _saveReaderServices)
                saveService.LoadProgress(_progressService.Progress);
        }

        private void InitializeMenuWindows()
        {
            var gameMenu = _windowService.GetWindow(WindowId.Hub).GetComponent<HubMenu>();
            gameMenu.gameObject.SetActive(true);
            gameMenu.PlayerLaunchedGame += OnPlayerLaunchedGame;

            foreach (OpenWindowButton button in gameMenu.GetComponentsInChildren<OpenWindowButton>()) 
                button.Construct(_windowService);
            
            /*var settingsMenu = _windowService.GetWindow(WindowId.Settings).GetComponent<SettingsMenu>();
            settingsMenu.MuteToggled += OnMuteToggled;*/
        }

        private void OnMuteToggled(object sender, bool e)
        {
            var audio = UnityEngine.Object.FindAnyObjectByType<AudioManager>();
            audio.IsMuted = e;
        }

        private void OnPlayerLaunchedGame(object sender, EventArgs e)
        {
            _stateMachine.Enter<LoadLevelState, string>("Level1");
        }
    }
}