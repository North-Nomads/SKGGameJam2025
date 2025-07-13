using System;
using HighVoltage.UI.Services.Factory;
using HighVoltage.UI.Services.GameWindows;
using HighVoltage.UI.Windows;

namespace HighVoltage.Infrastructure.States
{
    public class PlayerDiedState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;

        public PlayerDiedState(GameStateMachine gameStateMachine, IUIFactory uIFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uIFactory;
        }

        public void Enter()
        {
            var window = _uiFactory.InstantiateWindow(GameWindowId.PlayerDead);
            window.gameObject.SetActive(true);
            window.GetComponent<PlayerDeadWindow>().PlayerReturnedToMenu += ReturnToMenuPressed;
        }

        private void ReturnToMenuPressed(object sender, EventArgs e)
        {
            _gameStateMachine.Enter<HubState>();
        }

        public void Exit()
        {
            
        }
    }
}