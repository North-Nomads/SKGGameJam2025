using HighVoltage.Map.Tiles;
using HighVoltage.StaticData;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HighVoltage.Map
{
    public class TileGenerator : ITileGenerator
    {
        //vars to tweak
        private readonly int _tileSize = 64;

        private readonly Texture2D _tileAtlas;

        private string _savedMap;

        public TileGenerator(IStaticDataService staticDataService)
        {
            _tileAtlas = staticDataService.GetTileAtlas();
        }

        private void ReadSavedFile(int levelNumber)
        {
            var mapSave = Resources.Load($"Maps/{levelNumber}") as TextAsset;
            if (mapSave == null)
                throw new FileNotFoundException($"Maps/{levelNumber}");
            else
                _savedMap = mapSave.text;
            
        }

        public void LoadAndGenerateMap(int levelNumeber)
        {
            //destroy previous map?
            ReadSavedFile(levelNumeber);

            //ugh, yeah...
            int[] mapSize = _savedMap.Split('\n')[0].Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
            Texture2D mapTexture = new(mapSize[0] * _tileSize, mapSize[1] * _tileSize, TextureFormat.RGBA32, false);
            _savedMap = _savedMap.Split('\n')[1];

            if (mapSize[0] * mapSize[1] != _savedMap.Length)
                throw new Exception("IncorrectMapData");

            for(int i = 0; i < mapSize[1]; i++)
            {
                for(int j = 0; j < mapSize[0]; j++)
                {
                    int tileID = _savedMap[i*mapSize[0]+j] - '0';
                    //place waypoints and core items accordingly
                    Color[] pixels = _tileAtlas.GetPixels(tileID * _tileSize, 0, _tileSize, _tileSize);
                    mapTexture.SetPixels(j * _tileSize, i * _tileSize, _tileSize, _tileSize, pixels);
                }
            }

            mapTexture.Apply();
            Sprite mapSprite = Sprite.Create(mapTexture, new Rect(0, 0, mapTexture.width, mapTexture.height), new Vector2(0.5f, 0.5f), _tileSize);
            
            GameObject tilemapGameObject = new("Tilemap");
            var renderer = tilemapGameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = mapSprite;
        }
    }
}
