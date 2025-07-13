using UnityEngine;
using UnityEngine.UI;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private WindowId windowId;
        private Button _button;
        private IWindowService _windowService;

        public void Construct(IWindowService windowService) 
            => _windowService = windowService;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Open);
        }

        private void Open() 
            => _windowService.Open(windowId);
    }
}