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

        public void Initialize(SentryConfig config)
        {
            _sentryID = config.SentryId;
            buildingPrice.text = config.BuildPrice.ToString();
            itemIcon.sprite = config.SentryIcon;
            outline.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            outline.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
                outline.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            outline.enabled = !_isSelected;
            _isSelected = outline.enabled;
            OnCardSelected(this, _sentryID);
        }
    }
}