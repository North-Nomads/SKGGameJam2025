using System;
using UnityEngine;

namespace HighVoltage.Infrastructure.Tutorial
{
    [CreateAssetMenu(menuName = "Tutorial / TutorialScenario", fileName = "default tutorial", order = 10)]
    public class TutorialScenario : ScriptableObject
    {
        [SerializeField] private TutorialMessage[] tutorialMessages;

        public TutorialMessage[] TutorialMessages => tutorialMessages;
    }
    
    [Serializable]
    public class TutorialMessage
    {
        [SerializeField] private string message;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private TutorialEventType waitingForEvent;
        [SerializeField] private float timeToWaitIfNotForEvent;

        public string Message => message;
        public Sprite[] Sprites => sprites;

        public TutorialEventType WaitingForEvent => waitingForEvent;

        public float TimeToWaitIfNotForEvent => timeToWaitIfNotForEvent;
    }

    public enum TutorialEventType
    {
        None,
        WASD,
        LMB,
        RMB,
        MWL,
        Q,
        E,
        TAB,
        SentryEnabled,
        SentryDisabled,
        SentryDestroyed,
        FuseBoxPlaced
    }
}