using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using HighVoltage;

public class TileConverter : Editor
{
    [MenuItem("Assets/Convert to BuildableTiles", true)]
    public static bool ValidateConvert()
    {
        foreach (Object obj in Selection.objects)
            if (obj is Tile) return true;
        return false;
    }

    [MenuItem("Assets/Convert to BuildableTiles")]
    public static void ConvertToBuildableTiles()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Tile oldTile)
            {
                string path = AssetDatabase.GetAssetPath(oldTile);
                string dir = Path.GetDirectoryName(path);
                string name = Path.GetFileNameWithoutExtension(path);

                BuildableTile newTile = CreateInstance<BuildableTile>();
                newTile.sprite = oldTile.sprite;
                newTile.color = oldTile.color;
                newTile.colliderType = oldTile.colliderType;
                newTile.isBuildable = false;

                string newPath = AssetDatabase.GenerateUniqueAssetPath($"{dir}/{name}_Buildable.asset");
                AssetDatabase.CreateAsset(newTile, newPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Conversion complete.");
    }
}
