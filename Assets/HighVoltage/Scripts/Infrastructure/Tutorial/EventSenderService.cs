using System;

namespace HighVoltage.Infrastructure.Tutorial
{
    public class EventSenderService : IEventSenderService
    {
        public event EventHandler<TutorialEventType> OnEventHappened = delegate { };

        public void NotifyEventHappened(TutorialEventType tutorialEventType) 
            => OnEventHappened(this, tutorialEventType);
    }
}