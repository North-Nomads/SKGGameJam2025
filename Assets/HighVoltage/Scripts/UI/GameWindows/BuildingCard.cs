using System;
using HighVoltage.Infrastructure.Sentry;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class BuildingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event EventHandler<int> OnCardSelected = delegate { };
        
        [SerializeField] private Image outline;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI buildingPrice;
        private int _sentryID;
        private bool _isSelected;
        private bool _canBeAfforded;
        private int _itemPrice;

        public void Initialize(BuildingConfig config)
        {
            _sentryID = config.BuildingID;
            _itemPrice = config.BuildPrice;
            buildingPrice.text = config.BuildPrice.ToString();
            itemIcon.sprite = config.HUDIcon;
            outline.enabled = false;
            _canBeAfforded = false;
            gameObject.name = config.name;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_canBeAfforded)
                outline.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
                outline.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canBeAfforded)
                return;
            
            outline.enabled = !_isSelected;
            _isSelected = outline.enabled;
            OnCardSelected(this, _sentryID);
        }

        public void ToggleSelection(bool isSelected)
        {
            Debug.Log($"For {name} toggle selection {isSelected}");
            _isSelected = isSelected;
            outline.enabled = isSelected;
        }

        public void UpdatePurchasableStatus(int newMoney)
        {
            _canBeAfforded = newMoney >= _itemPrice;
            if (!_canBeAfforded)
                outline.enabled = false;
        }
    }
}