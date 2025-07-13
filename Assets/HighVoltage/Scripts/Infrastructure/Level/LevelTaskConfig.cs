using UnityEngine;

namespace HighVoltage.Level
{
    [CreateAssetMenu(fileName = "LevelTask_", menuName = "Config/LevelTask", order = 2)]
    public class LevelTaskConfig : ScriptableObject
    {
        [SerializeField] private int taskID;
        [SerializeField] private string taskTitle;
        [SerializeField] private string taskDescription;
        [SerializeField] private int secondsToComplete;
        [SerializeField] private bool noDamageChallenge;

        public int TaskID => taskID;
        public int SecondsToComplete => secondsToComplete;
        public string TaskDescription => taskDescription;
        public string TaskTitle => taskTitle;
        public bool NoDamageChallenge => noDamageChallenge;
    }
}