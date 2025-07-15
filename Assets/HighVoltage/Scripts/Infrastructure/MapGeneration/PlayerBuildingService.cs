using HighVoltage.Map.Building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HighVoltage
{
    public class PlayerBuildingService : IPlayerBuildingService
    {
        public Tilemap MapTilemap { get; set; }


        public void BuildStructure(Vector3 worldCoordinates, GameObject building)
        {
            TileBase tile = MapTilemap.GetTile(MapTilemap.WorldToCell(worldCoordinates));
            if (tile is not BuildableTile buildableTile || !buildableTile.isBuildable)
                return;
            Build(MapTilemap.GetCellCenterWorld(MapTilemap.WorldToCell(worldCoordinates)), building);
        }

        private void Build(Vector3 cellCoordinates, GameObject building)
        {
            Object.Instantiate(building, cellCoordinates, new Quaternion(0,0,0,0));
            
        }
    }
}
