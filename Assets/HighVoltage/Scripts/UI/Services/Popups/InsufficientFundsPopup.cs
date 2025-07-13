using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.PopUps
{
    public class InsufficientFunds : PopupWindow
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button goToShopButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(ClosePopUp);
            //goToShopButton.onClick.AddListener(OpenShopWindow);
        }

        private void ClosePopUp()
            => WindowService.CloseCurrentPopUp();
    }
}