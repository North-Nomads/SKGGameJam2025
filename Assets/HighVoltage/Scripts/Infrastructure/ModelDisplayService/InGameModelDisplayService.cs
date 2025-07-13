using HighVoltage.Infrastructure.Factory;
using HighVoltage.Services.Progress;

namespace HighVoltage.Infrastructure.ModelDisplayService
{
    public class InGameModelDisplayService : IModelDisplayService
    {
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerProgressService _playerProgress;
        private HubModelDisplayer _displayer;

        public InGameModelDisplayService(IGameFactory gameFactory, IPlayerProgressService playerProgress)
        {
            _gameFactory = gameFactory;
            _playerProgress = playerProgress;
        }

        public void OnHubLoading()
        {
            _displayer = _gameFactory.CreateModelDisplayer();
            _displayer.Construct(_gameFactory, _playerProgress);
        }
    }
}