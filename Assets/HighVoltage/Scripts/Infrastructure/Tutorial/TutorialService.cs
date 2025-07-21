using System;
using System.Collections;
using HighVoltage.Map.Building;
using HighVoltage.Services.Progress;
using HighVoltage.StaticData;
using UnityEngine;

namespace HighVoltage.Infrastructure.Tutorial
{
    public class TutorialService : ITutorialService
    {
        public event EventHandler<TutorialMessage> UserRequiresNewTutorialStep = delegate { };
        public event EventHandler AllTutorialStepsFinished = delegate { };

        private readonly IEventSenderService _eventSenderService;
        private readonly IPlayerBuildingService _buildingService;
        private readonly IPlayerProgressService _playerProgress;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly TutorialScenario _scenario;
        private TutorialMessage _currentScenarioStep;
        private int _scenarioStepIndex;

        public TutorialService(IStaticDataService staticDataService, IEventSenderService eventSenderService, 
            ICoroutineRunner coroutineRunner, IPlayerProgressService playerProgress, 
            IPlayerBuildingService buildingService)
        {
            _coroutineRunner = coroutineRunner;
            _playerProgress = playerProgress;
            _buildingService = buildingService;
            _eventSenderService = eventSenderService;
            _eventSenderService.OnEventHappened += OnGameEventOccured;
            _scenario = staticDataService.GetTutorialScenario();
            
            Debug.Log($"Loaded tutorial scenario with {_scenario.TutorialMessages.Length} steps");
            _currentScenarioStep = _scenario.TutorialMessages[0];
            _scenarioStepIndex = 0;
        }

        private void OnGameEventOccured(object sender, TutorialEventType e)
        {
            if (e != TutorialEventType.None && e == _currentScenarioStep.WaitingForEvent)
                TutorialStepCompleted();
        }

        public void StartTutorial()
        {
            UserRequiresNewTutorialStep(this, _currentScenarioStep);
            
            if (_currentScenarioStep.WaitingForEvent != TutorialEventType.None)
                return;
            _coroutineRunner.StartCoroutine(WaitForNextTutorialStep());
        }

        public void TutorialStepCompleted()
        {
            Debug.Log("Tutorial step completed");
            _scenarioStepIndex++;
            if (_scenarioStepIndex >= _scenario.TutorialMessages.Length)
            {
                TutorialFinished();
                return;
            }

            _currentScenarioStep = _scenario.TutorialMessages[_scenarioStepIndex];
            UserRequiresNewTutorialStep(this, _currentScenarioStep);

            if (_currentScenarioStep.WaitingForEvent != TutorialEventType.None)
                return;
            _coroutineRunner.StartCoroutine(WaitForNextTutorialStep());
        }

        private void TutorialFinished()
        {
            _playerProgress.Progress.HasFinishedTutorial = true;
            _buildingService.ToggleBuildingAllowance(false);
            AllTutorialStepsFinished(this, null);
        }

        private IEnumerator WaitForNextTutorialStep()
        {
            Debug.Log($"Waiting for next tutorial step for {_currentScenarioStep.TimeToWaitIfNotForEvent}s");
            yield return new WaitForSeconds(_currentScenarioStep.TimeToWaitIfNotForEvent);
            TutorialStepCompleted();
        }
    }
}