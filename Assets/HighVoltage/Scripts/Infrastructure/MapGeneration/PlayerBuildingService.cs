using HighVoltage.Infrastructure.Factory;
using HighVoltage.Infrastructure.MobSpawning;
using HighVoltage.Infrastructure.Sentry;
using HighVoltage.Map.Building;
using HighVoltage.StaticData;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HighVoltage
{
    public class PlayerBuildingService : IPlayerBuildingService
    {
        private int _selectedSentry;
        private ICurrentReceiver _selectedReciever;
        private ICurrentSource _selectedSource;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;
        private readonly IMobSpawnerService _mobSpawnerService;

        
        public Tilemap MapTilemap { get; set; }

        public PlayerBuildingService(IStaticDataService staticDataService, IGameFactory gameFactory, IMobSpawnerService mobSpawnerService)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _mobSpawnerService = mobSpawnerService;
            _selectedSentry = 1;
        }

        public void BuildStructure(Vector3 worldCoordinates)
        {
            TileBase tile = MapTilemap.GetTile(MapTilemap.WorldToCell(worldCoordinates));
            if (tile is not BuildableTile buildableTile || !buildableTile.isBuildable)
                return;

            SentryConfig sentryConfig = _staticDataService.ForSentryID(_selectedSentry);
            SentryTower sentryTower = _gameFactory.CreateSentry(MapTilemap.WorldToCell(worldCoordinates), sentryConfig);
            sentryTower.Initialize(sentryConfig, _mobSpawnerService, _gameFactory);
        }

        public void SelectedSentryChanged(int selectedSentry)
            => _selectedSentry = selectedSentry;

        public void SelectTargetForWiring(ICurrentObject building)
        {
            if(building is ICurrentReceiver receiver)
                _selectedReciever = receiver;
            else if(building is ICurrentSource source)
                _selectedSource = source;

            if(_selectedSource != null && _selectedReciever != null)
            {
                _selectedReciever.AttachToSource(_selectedSource);
                _selectedSource.AttachReceiver(_selectedReciever);
            }
        }

        public void SelectTargetForUnwiring(ICurrentObject building)
        {
            //I went for 2in1 approach on deselction
            //not the best Idea, oh well
            if(building == _selectedReciever)
                _selectedReciever = null;
            if(building == _selectedSource)
                _selectedSource = null;

            if (building is ICurrentSource source)
                source.DetachAllReceivers();
            else if (building is ICurrentReceiver receiver)
            {
                //i didn't feel like adding DetachFromSource, mb
                receiver.CurrentProvider.DetachReceiver(receiver);
                receiver.AttachToSource(null);
            }
        }
    }
}
