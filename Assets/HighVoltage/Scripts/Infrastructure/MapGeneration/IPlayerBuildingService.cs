using HighVoltage.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HighVoltage.Map.Building
{
    public interface IPlayerBuildingService : IService
    {
        public Tilemap MapTilemap { get; set; }
        public void BuildStructure(Vector3 worldCoordinates, GameObject building);
        
    }
}
