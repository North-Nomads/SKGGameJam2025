using System;
using HighVoltage.Infrastructure.Factory;
using UnityEngine;
using HighVoltage.Services;
using HighVoltage.Infrastructure.Interactables;
using Random = UnityEngine.Random;
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
            var nextLevelPortalSpawnPoint = GameObject.FindGameObjectWithTag(Constants.NextLevelPortalSpawnPoint);
            NextLevelPortal nextLevelPortal = _gameFactory.CreateNextLevelPortal(at: nextLevelPortalSpawnPoint);
            nextLevelPortal.DelayAfterPortalJumpExpired += SwitchSceneAfterJumpDelay;
        }

        private void SwitchSceneAfterJumpDelay(object sender, EventArgs e)
        {
            if (_progressService.Progress.RemainingTasks == 0)
            {
                _gameStateMachine.Enter<LoadLevelState, string>($"{Constants.BossSceneName}");
            }
            else
            {
                _gameStateMachine.Enter<LoadLevelState, string>($"{Constants.GameplayScene}{Random.Range(1, Constants.GameplayScenesCount)}");
            }
        }

        private void ReturnToHub(object sender, EventArgs e) 
            => _gameStateMachine.Enter<HubState>();

        public void Exit() 
        {
        }
    }
}