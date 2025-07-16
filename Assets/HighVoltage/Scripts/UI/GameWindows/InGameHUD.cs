using System.Collections.Generic;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Map.Building;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class InGameHUD : GameWindowBase
    {
        [SerializeField] private Transform buildingCardParent;
        [SerializeField] private Image playerCoreHealthBar;
        private PlayerCore _playerCore;

        public override void OnOpened()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(buildingCardParent.GetComponent<RectTransform>());
        }

        public void ProvideSceneData(PlayerCore playerCore,
            IPlayerBuildingService buildingService)
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
        {
            print(
                $"Handling... Current health: {currentHealth} / {_playerCore.MaxCoreHealth}. Result: {(float)currentHealth / _playerCore.MaxCoreHealth}");
            playerCoreHealthBar.fillAmount = (float)currentHealth / _playerCore.MaxCoreHealth;
        }
    }
}