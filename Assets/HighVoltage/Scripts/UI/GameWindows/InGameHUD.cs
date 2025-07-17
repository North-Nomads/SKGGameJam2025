using System;
using System.Collections.Generic;
using HighVoltage.Infrastructure.BuildingStore;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Map.Building;
using HighVoltage.Services;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class InGameHUD : GameWindowBase
    {
        [SerializeField] private TextMeshProUGUI nextWaveTimer;
        [SerializeField] private TextMeshProUGUI playerWallet;
        [SerializeField] private Transform buildingCardParent;
        [SerializeField] private Button timerSkipButton;
        [SerializeField] private Image playerCoreHealthBar;
        private PlayerCore _playerCore;
        private float _delayTimeLeft;
        private List<BuildingCard> _buildingCards;
        
        public event EventHandler NextWaveTimerIsUp = delegate { };

        protected override void OnStart()
        {
            base.OnStart();
            timerSkipButton.onClick.AddListener(HandlePreparationTimeSkip);
        }

        private void HandlePreparationTimeSkip()
        {
            _delayTimeLeft = Constants.TimeLeftAfterPreparationTimeSkip;
            timerSkipButton.gameObject.SetActive(false);
        }

        public override void OnOpened()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(buildingCardParent.GetComponent<RectTransform>());
        }

        public void SetNextWaveTimer(float seconds)
        {
            _delayTimeLeft = seconds;
            nextWaveTimer.gameObject.SetActive(true);
            timerSkipButton.gameObject.SetActive(true);
        }

        public void ProvideSceneData(PlayerCore playerCore, IPlayerBuildingService buildingService, 
            IBuildingStoreService buildingStore)
        {
            _buildingCards = new List<BuildingCard>();
            SetupPlayerCoreObserver();
            BuildBuildingUI();
            playerWallet.text = buildingStore.MoneyPlayerHas.ToString();
            
            buildingStore.CurrencyChanged += (_, newMoney) =>
            {
                playerWallet.text = newMoney.ToString();
                foreach (BuildingCard buildingCard in _buildingCards) 
                    buildingCard.UpdatePurchasableStatus(newMoney);
            };  
            return;

            void BuildBuildingUI()
            {
                foreach (SentryConfig sentry in LevelProgress.GetSentriesForThisLevel())
                {
                    BuildingCard buildingCard = GameWindowService.CreateBuildingCard(sentry, buildingCardParent);
                    buildingCard.OnCardSelected += (sender, selectedSentry) =>
                    {
                        foreach (BuildingCard card in _buildingCards)
                        {
                            card.ToggleSelection(false);    
                        }

                        ((BuildingCard)sender).ToggleSelection(true);
                        buildingService.SelectedSentryChanged(selectedSentry);
                    };
                    _buildingCards.Add(buildingCard);
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
            if (_delayTimeLeft <= Constants.TimeLeftAfterPreparationTimeSkip && nextWaveTimer.gameObject.activeSelf)
            {
                timerSkipButton.gameObject.SetActive(false);
            }
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
            int seconds = Mathf.FloorToInt(_delayTimeLeft % 60f);
            nextWaveTimer.text = $"{seconds + 1:00}";
        }
    }
}