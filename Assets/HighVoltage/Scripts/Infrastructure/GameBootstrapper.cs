using System.Collections;
using UnityEngine;
using HighVoltage.Infrastructure.States;

namespace HighVoltage.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Canvas loadCurtain;
        private Game _game;


        private void Awake()
        {
            _game = new Game(this, loadCurtain);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(loadCurtain);
        }
    }
}