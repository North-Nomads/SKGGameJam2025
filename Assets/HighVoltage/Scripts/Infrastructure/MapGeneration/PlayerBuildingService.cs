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
    }
}
