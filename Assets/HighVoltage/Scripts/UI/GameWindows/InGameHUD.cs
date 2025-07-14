using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class InGameHUD : GameWindowBase
    {
        [SerializeField] private Image playerCoreHealthBar;
        private PlayerCore _playerCore;
        
        public void ProvidePlayerCore(PlayerCore playerCore)
        {
            _playerCore = playerCore;
            playerCoreHealthBar.fillAmount = 1;
            _playerCore.OnCoreHealthChanged += HandleCoreHealthChanged;
        }

        private void HandleCoreHealthChanged(object sender, int currentHealth)
        {
            print($"Handling... Current health: {currentHealth} / {_playerCore.MaxCoreHealth}. Result: {(float) currentHealth / _playerCore.MaxCoreHealth}");
            playerCoreHealthBar.fillAmount = (float) currentHealth / _playerCore.MaxCoreHealth;
        }
    }
}