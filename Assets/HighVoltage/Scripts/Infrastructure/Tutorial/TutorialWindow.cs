using System.Collections.Generic;
using HighVoltage.Infrastructure.Services;
using HighVoltage.UI.Services.Factory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HighVoltage.Infrastructure.Tutorial
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Transform controlImagesParent;
        private IUIFactory _uiFactory;

        private Dictionary<TutorialMessage, List<Image>> _tutorialImageScreens;
        private List<Image> _openedTutorialImageScreen;

        private void Awake()
        {
            _uiFactory = AllServices.Container.Single<IUIFactory>();
            _openedTutorialImageScreen = new List<Image>();
            _tutorialImageScreens = new Dictionary<TutorialMessage, List<Image>>();
        }

        private void OnEnable()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(controlImagesParent.GetComponent<RectTransform>());
        }

        public void InitializeMessage(TutorialMessage message)
        {
            List<Image> tutorialStepImages = _uiFactory.CreateTutorialImages(message.Sprites, controlImagesParent);
            foreach (Image image in tutorialStepImages)
                image.gameObject.SetActive(false);
            _tutorialImageScreens.Add(message, tutorialStepImages);
        }

        public void DisplayMessage(TutorialMessage message)
        {
            text.text = message.Message;
            
            // hide all opened images
            if (_openedTutorialImageScreen.Count != 0)
                foreach (Image image in _openedTutorialImageScreen)
                    image.gameObject.SetActive(false);
            
            // display images according to new step
            foreach (Image image in _tutorialImageScreens[message])
                image.gameObject.SetActive(true);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(controlImagesParent.GetComponent<RectTransform>());
            _openedTutorialImageScreen = _tutorialImageScreens[message];
        }

    }
}