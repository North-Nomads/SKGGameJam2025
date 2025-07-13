using TMPro;
using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Windows;
using HighVoltage.UI.Windows;

namespace HighVoltage.Assets.Wizard.Scripts.UI.Windows
{
    public class HubHUDWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _diamondsText;

        public override void ConstructWindow(IPlayerProgressService progressService,
                                             WindowId windowId, IWindowService windowService, ISaveLoadService saveLoadService,
                                             IGameFactory gameFactory)
        {
            base.ConstructWindow(progressService, windowId, windowService, saveLoadService, gameFactory);
        }
    }
}
