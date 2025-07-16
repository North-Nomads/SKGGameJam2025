using System;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Map.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class InGameHUD : GameWindowBase
    {
        [SerializeField] private TextMeshProUGUI nextWaveTimer;
        [SerializeField] private Transform buildingCardParent;
        [SerializeField] private Image playerCoreHealthBar;
        private PlayerCore _playerCore;
        private float _delayTimeLeft;

        public event EventHandler NextWaveTimerIsUp = delegate { };

        public override void OnOpened()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(buildingCardParent.GetComponent<RectTransform>());
        }

        public void SetNextWaveTimer(float seconds)
        {
            _delayTimeLeft = seconds;
            nextWaveTimer.gameObject.SetActive(true);
        }

        public void ProvideSceneData(PlayerCore playerCore, IPlayerBuildingService buildingService)
        {
            SetupPlayerCoreObserver();
            BuildBuildingUI();
            return;

            void BuildBuildingUI()
            {
                foreach (SentryConfig sentry in LevelProgress.GetSentriesForThisLevel())
                {
                    BuildingCard buildingCard = GameWindowService.CreateBuildingCard(sentry, buildingCardParent);
                    buildingCard.OnCardSelected += (sender, selectedSentry) =>
                        buildingService.SelectedSentryChanged(selectedSentry);
                }
            }

            void SetupPlayerCoreObserver()
            {
                _playerCore = playerCore;
                playerCoreHealthBar.fillAmount = 1;
                _playerCore.OnCoreHealthChanged += HandleCoreHealthChanged;
            }
        }

        private void HandleCoreHealthChanged(object sender, int currentHealth) 
            => playerCoreHealthBar.fillAmount = (float)currentHealth / _playerCore.MaxCoreHealth;

        private void Update()
        {
            if (_delayTimeLeft <= 0)
                return;
            
            _delayTimeLeft -= Time.deltaTime;
            if (_delayTimeLeft < 0)
            {
                _delayTimeLeft = 0;
                NextWaveTimerIsUp(null, null);
                nextWaveTimer.gameObject.SetActive(false);
            }

            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(_delayTimeLeft / 60f);
            int seconds = Mathf.FloorToInt(_delayTimeLeft % 60f);
        
            // Format as mm:ss with leading zeros
            nextWaveTimer.text = $"{minutes:00}:{seconds:00}";
        }
    }
}