using HighVoltage.Infrastructure.Sentry;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.UI.GameWindows
{
    public class BuildingCard : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI buildingPrice;

        public void Initialize(SentryConfig config)
        {
            buildingPrice.text = config.BuildPrice.ToString();
            itemIcon.sprite = config.SentryIcon;
        }
    }
}