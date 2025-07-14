using UnityEngine;
using HighVoltage.Infrastructure.Services;
using HighVoltage.Infrastructure.States;

namespace HighVoltage.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, Canvas loadCurtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadCurtain, AllServices.Container, coroutineRunner);
        }
    }
}
