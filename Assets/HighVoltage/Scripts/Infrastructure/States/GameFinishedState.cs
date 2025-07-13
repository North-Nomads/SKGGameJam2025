using System;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.States
{
    public class GameFinishedState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerProgressService _progressService;

        public GameFinishedState(GameStateMachine gameStateMachine, IGameFactory gameFactory, IPlayerProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            
        }

        private void ReturnToHub(object sender, EventArgs e) 
            => _gameStateMachine.Enter<HubState>();

        public void Exit() 
        {
        }
    }
}