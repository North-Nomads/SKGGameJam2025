using HighVoltage.Infrastructure.BuildingStore;
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
        private int _selectedBuildingID;
        private ICurrentReceiver _selectedReceiver;
        private ICurrentSource _selectedSource;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;
        private readonly IMobSpawnerService _mobSpawnerService;
        private readonly IBuildingStoreService _buildingStoreService;

        public Tilemap MapTilemap { get; set; }

        public PlayerBuildingService(IStaticDataService staticDataService, IGameFactory gameFactory,
            IMobSpawnerService mobSpawnerService, IBuildingStoreService buildingStoreService)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _mobSpawnerService = mobSpawnerService;
            _buildingStoreService = buildingStoreService;
            _selectedBuildingID = 1;
        }

        public void BuildStructure(Vector3 worldCoordinates)
        {
            TileBase tile = MapTilemap.GetTile(MapTilemap.WorldToCell(worldCoordinates));
            if (tile is not BuildableTile { isBuildable: true })
                return;

            SentryConfig sentryConfig = _staticDataService.ForSentryID(_selectedBuildingID);
            SwitchConfig switchConfig = _staticDataService.ForSwitchID(_selectedBuildingID);
            
            BuildingConfig thingToBuild = sentryConfig == null ? switchConfig : sentryConfig;
            
            if (!_buildingStoreService.CanAfford(thingToBuild))
                return;

            if (sentryConfig == null)
            {
                SwitchMain switchMain =
                    _gameFactory.CreateSwitch(MapTilemap.WorldToCell(worldCoordinates), switchConfig);
            }
            else
            {
                SentryTower sentryTower = _gameFactory.CreateSentry(MapTilemap.WorldToCell(worldCoordinates), sentryConfig);
                sentryTower.Initialize(sentryConfig, _mobSpawnerService, _gameFactory);    
            }
            
            
            _buildingStoreService.SpendMoneyOn(thingToBuild);
        }

        public void SelectedSentryChanged(int selectedSentry)
            => _selectedBuildingID = selectedSentry;

        public void SelectTargetForWiring(ICurrentObject building)
        {
            if(building is ICurrentReceiver receiver)
                _selectedReceiver = receiver;
            else if(building is ICurrentSource source)
                _selectedSource = source;

            if(_selectedSource != null && _selectedReceiver != null)
            {
                _selectedReceiver.AttachToSource(_selectedSource);
                _selectedSource.AttachReceiver(_selectedReceiver);
            }
        }

        public void SelectTargetForUnwiring(ICurrentObject building)
        {
            //I went for 2in1 approach on deselction
            //not the best Idea, oh well
            if(building == _selectedReceiver)
                _selectedReceiver = null;
            if(building == _selectedSource)
                _selectedSource = null;

            if (building is ICurrentSource source)
                source.DetachAllReceivers();
            else if (building is ICurrentReceiver receiver)
            {
                //i didn't feel like adding DetachFromSource, mb
                receiver.CurrentSource.DetachReceiver(receiver);
                receiver.AttachToSource(null);
            }
        }
    }
}
