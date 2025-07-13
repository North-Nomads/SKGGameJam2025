using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HighVoltage.Infrastructure.MobSpawnerService;
using HighVoltage.Level;

namespace HighVoltage.UI.GameWindows
{
    public class InGameHUD : GameWindowBase
    {
        [SerializeField] private Image[] healthPoints;
        [SerializeField] private TextMeshProUGUI taskTitle;
        [SerializeField] private TextMeshProUGUI taskDescription;
        [SerializeField] private TextMeshProUGUI taskStateLabel;
        [SerializeField] private TextMeshProUGUI taskTime;
        [SerializeField] private TextMeshProUGUI progressTitle;
        [SerializeField] private TextMeshProUGUI progressDescription;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image progressBackground;
        [SerializeField] private Color missionCompleteColor;
        [SerializeField] private Color missionFailedColor;
        [SerializeField] private Color missionIndeterminateColor;
        [SerializeField] private string progressTextFormat;
        [SerializeField] private string taskCompletedText;
        [SerializeField] private string taskFailedText;

        private bool _isCurrentTaskTimed = false;
        private float _secondsLeft;

        public void UpdateTask(LevelTaskConfig levelTask, int remainingTasks)
        {
            taskTitle.text = levelTask.TaskTitle;
            taskDescription.text = levelTask.TaskDescription;
            _secondsLeft = levelTask.SecondsToComplete;
            _isCurrentTaskTimed = levelTask.SecondsToComplete > 0;
            UpdateRemainingTasks(remainingTasks);

            if (!_isCurrentTaskTimed)
                taskTime.gameObject.SetActive(false);

            UpdateTaskTimer();
        }

        public void UpdateOnLevelFinished(bool shouldGiveReward, int remainingTasks)
        {
            taskStateLabel.text = shouldGiveReward ? taskCompletedText : taskFailedText;
            backgroundImage.color = shouldGiveReward ? missionCompleteColor : missionFailedColor;
            UpdateRemainingTasks(remainingTasks);
        }

        private void UpdateRemainingTasks(int remainingTasks)
        {
            progressDescription.text = Regex.Replace(progressTextFormat, @"\{[\d\w_]+\}", remainingTasks.ToString());
            progressBackground.color = remainingTasks > 0 ? missionIndeterminateColor : missionCompleteColor;
        }

        private void UpdateTaskTimer() 
            => taskTime.text = SecondsToString(_secondsLeft);

        private void Update()
        {
            if (!_isCurrentTaskTimed)
                return; 
            
            _secondsLeft -= Time.deltaTime;
            UpdateTaskTimer();
        }

        private string SecondsToString(float secondsToComplete)
        {
            int seconds = (int)(secondsToComplete % 60);
            int minutes = (int)(secondsToComplete / 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }
}