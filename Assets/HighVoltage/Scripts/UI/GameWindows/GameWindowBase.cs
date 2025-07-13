using UnityEngine;
using HighVoltage.Level;
using HighVoltage.UI.Services;

namespace HighVoltage.UI.GameWindows
{
    public abstract class GameWindowBase : MonoBehaviour
    {
        protected IGameWindowService GameWindowService;
        protected ILevelProgress LevelProgress;

        public virtual void ConstructWindow(IGameWindowService gameWindowService, ILevelProgress levelProgress)
        {
            GameWindowService = gameWindowService;
            LevelProgress = levelProgress;
        }

        public virtual void OnOpened() { }
        public virtual void OnClosed() { }

        protected void CloseWindow()
            => GameWindowService.ReturnToPreviousWindow();

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