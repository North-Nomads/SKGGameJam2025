using HighVoltage.Infrastructure.Tutorial;
using HighVoltage.Services;
using HighVoltage.StaticData;
using HighVoltage.UI.Services.Factory;
using UnityEngine;

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

        public LoadTutorialState(GameStateMachine gameStateMachine, IUIFactory uiFactory, Canvas loadingCurtain,
            IStaticDataService staticDataService, ITutorialService tutorialService, SceneLoader sceneLoader)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
            _tutorialService = tutorialService;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.TutorialSceneName, onLoaded: OnLoaded);
        }

        private void OnLoaded()
        {
            _uiFactory.CreateUIRoot();
            TutorialWindow tutorialWindow = _uiFactory.InstantiateTutorialMessageBox();
            TutorialScenario tutorialScenario = _staticDataService.GetTutorialScenario();
            foreach (TutorialMessage message in tutorialScenario.TutorialMessages)
                tutorialWindow.InitializeMessage(message);
            _tutorialService.UserRequiresNewTutorialStep +=
                (_, tutorialMessage) =>
                {
                    tutorialWindow.DisplayMessage(tutorialMessage);
                };
            _gameStateMachine.Enter<TutorialLoopState>();
        }

        public void Exit()
        {
            _loadingCurtain.gameObject.SetActive(false);
        }
    }
}