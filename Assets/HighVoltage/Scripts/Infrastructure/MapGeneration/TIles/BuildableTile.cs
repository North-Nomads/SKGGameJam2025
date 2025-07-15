using UnityEngine;
using UnityEngine.Tilemaps;

namespace HighVoltage
{

    [CreateAssetMenu(menuName = "2D/Tiles/Buildable Tile")]
    public class BuildableTile : Tile
    {
        public bool isBuildable = true;
    }
}
