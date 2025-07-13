using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.HubVisuals;
using HighVoltage.Infrastructure.ModelDisplayService;
using HighVoltage.Infrastructure.SaveLoad;
using HighVoltage.Services.Progress;
using HighVoltage.UI.Services.Windows;

namespace HighVoltage.UI.Windows
{

    public abstract class WindowBase : MonoBehaviour
    {
        protected IPlayerProgressService PlayerProgress;
        protected WindowId ThisWindowId;
        protected IWindowService WindowService;
        protected ISaveLoadService SaveLoad;
        protected IGameFactory GameFactory;
        protected IModelDisplayService ModelDisplayService;
        protected IHubVFX HubVFX;

        public virtual void ConstructWindow(IPlayerProgressService progressService,
            WindowId windowId, IWindowService windowService, ISaveLoadService saveLoadService, IGameFactory gameFactory,
            IModelDisplayService _modelDisplayService, IHubVFX hubVFX)
        {
            PlayerProgress = progressService;
            ThisWindowId = windowId;
            WindowService = windowService;
            SaveLoad = saveLoadService;
            GameFactory = gameFactory;
            ModelDisplayService = _modelDisplayService;
            HubVFX = hubVFX;
        }

        public virtual void OnOpened() { }
        protected void CloseWindow() 
            => WindowService.ReturnToPreviousWindow();
        protected void ReturnToHub()
            => WindowService.Open(WindowId.Hub);
        protected virtual void OnStart()
        {
            Initialize();
            SubscribeUpdates();
        }
        protected virtual void CleanUp() { }
        protected virtual void Initialize() { }
        protected virtual void SubscribeUpdates() { }
        
        private void Start() 
            => OnStart();

        private void OnDestroy()
            => CleanUp();
    }
}