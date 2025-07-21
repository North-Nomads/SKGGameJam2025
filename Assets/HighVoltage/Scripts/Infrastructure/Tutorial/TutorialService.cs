using System;
using HighVoltage.StaticData;
using UnityEngine;

namespace HighVoltage.Infrastructure.Tutorial
{
    public class TutorialService : ITutorialService
    {
        public event EventHandler<TutorialMessage> UserRequiresNewTutorialStep = delegate { };
        
        private readonly IStaticDataService _staticDataService;
        private readonly TutorialScenario _scenario;
        private TutorialMessage _currentScenarioStep;
        private int _scenarioStepIndex;

        public TutorialService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _scenario = _staticDataService.GetTutorialScenario();
            Debug.Log($"Loaded tutorial scenario with {_scenario.TutorialMessages.Length} steps");
            _currentScenarioStep = _scenario.TutorialMessages[0];
            _scenarioStepIndex = 0;
        }
        
        public void StartTutorial()
        {
            UserRequiresNewTutorialStep(this, _currentScenarioStep);
        }

        public void TutorialStepCompleted()
        {
            _scenarioStepIndex++;
            _currentScenarioStep = _scenario.TutorialMessages[_scenarioStepIndex];
            UserRequiresNewTutorialStep(this, _currentScenarioStep);
        }
    }
}