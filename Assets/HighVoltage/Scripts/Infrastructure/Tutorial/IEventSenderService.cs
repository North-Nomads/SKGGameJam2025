using System;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.Tutorial
{
    public interface IEventSenderService : IService
    {
        event EventHandler<TutorialEventType> OnEventHappened;
        void NotifyEventHappened(TutorialEventType tutorialEventType);
    }
}