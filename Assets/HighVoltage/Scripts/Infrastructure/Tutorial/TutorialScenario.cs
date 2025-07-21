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

        public string Message => message;
        public Sprite[] Sprites => sprites;
    }
}