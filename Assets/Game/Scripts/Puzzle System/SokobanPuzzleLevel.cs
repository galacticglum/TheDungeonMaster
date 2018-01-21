/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleLevel.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/08/18
 * Modified Date: 01/20/18
 * Description: Data structure for Sokoban levels. 
 */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Data structure for Sokoban levels.
/// </summary>
[CreateAssetMenu]
public class SokobanPuzzleLevel : ScriptableObject
{
    private const float MetadataCrateAlpha = 128 / 255f; 

    private static readonly Color WallTileColour = Color.black; 
    private static readonly Color GoalTileColour = Color.green;  

    /// <summary>
    /// The size of our <see cref="SokobanPuzzleLevel"/> in a <see cref="Vector2Int"/>.
    /// </summary>
    public Vector2Int Size => size;

    /// <summary>
    /// Indicates whether the tilemap has been built.
    /// </summary>
    public bool HasGeneratedTiles => tiles != null;

    [SerializeField]
    private SokobanPuzzleTile[] tiles;

    [SerializeField]
    private Vector2Int size;

    /// <summary>
    /// Builds an empty tilemap.
    /// </summary>
    public void GenerateTiles(Vector2Int size, SokobanTileType defaulType = SokobanTileType.Floor)
    {
        tiles = new SokobanPuzzleTile[size.x * size.y];

        // Initialize each tile
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                tiles[y * Size.x + x] = new SokobanPuzzleTile(defaulType);
            }
        }

        EditorUtility.SetDirty(this);
    }

    public void LoadFromFile(string filePath)
    {
        // Load the file into a texture
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D imageTexture = new Texture2D(2, 2);

        if (!imageTexture.LoadImage(imageBytes))
        {
            Debug.LogError("LevelLoader::Load: Could not load level because file is invalid!");
            Application.Quit();
            return;
        }

        GenerateTiles(new Vector2Int(imageTexture.width, imageTexture.height));

        Color[] colours = imageTexture.GetPixels();
        for (int x = 0; x < imageTexture.width; x++)
        {
            for (int y = 0; y < imageTexture.height; y++)
            {
                Color colour = colours[x + imageTexture.width * y];
                float alpha = colour.a;

                colour.a = 1;

                SokobanPuzzleTile tile = GetTileAt(x, y);
                SokobanTileType type = SokobanTileType.Floor;
                if (colour == WallTileColour)
                {
                    type = SokobanTileType.Wall;
                }
                else if (colour == GoalTileColour)
                {
                    type = SokobanTileType.Goal;
                }

                tile.Type = type;
                tile.SpawnCrate = alpha == MetadataCrateAlpha && IsValidSpawn(type);
            }
        }

        EditorUtility.SetDirty(this);
    }

    /// <summary>
    /// Get a tile at a specified coordinate.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">Thrown if the coordinate is out of range.</exception>
    public SokobanPuzzleTile GetTileAt(int x, int y)
    {
        if (IsOutOfRange(x, y))
        {
            throw new IndexOutOfRangeException();
        }

        return tiles[y * Size.x + x];
    }

    /// <summary>
    /// Checks if the specified position is out-of-range range of the tile array.
    /// </summary>
    public bool IsOutOfRange(int x, int y) => x < 0 || x >= Size.x || y < 0 || y >= Size.y;

    private static bool IsValidSpawn(SokobanTileType type) => type != SokobanTileType.Wall;
}
