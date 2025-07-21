using System;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.Tutorial
{
    public interface ITutorialService : IService
    {
        event EventHandler<TutorialMessage> UserRequiresNewTutorialStep;
        void StartTutorial();
        void TutorialStepCompleted();
        event EventHandler AllTutorialStepsFinished;
        void InterruptTutorial();
    }
}