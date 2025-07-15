using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        
        private Button _button;
        private int _levelIndex;
        public event EventHandler<int> LevelButtonPressed = delegate { };

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => LevelButtonPressed(this, _levelIndex));
        }

        public void Construct(int levelIndex)
        {
            _levelIndex = levelIndex;
            buttonText.text = _levelIndex.ToString();
        }
    }
}
