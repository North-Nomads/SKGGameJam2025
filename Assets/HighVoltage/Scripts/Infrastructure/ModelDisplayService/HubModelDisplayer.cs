using UnityEngine;
using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.ModelDisplayService
{
    public class HubModelDisplayer : MonoBehaviour
    {
        private IPlayerProgressService _progressService;
        private IGameFactory _gameFactory;

        public void Construct(IGameFactory gameFactory, IPlayerProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
        }
    }
}