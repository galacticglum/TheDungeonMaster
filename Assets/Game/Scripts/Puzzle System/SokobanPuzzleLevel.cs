/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleLevel.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/08/18
 * Modified Date: 01/18/18
 * Description: Data structure for Sokoban levels. 
 */

using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Data structure for Sokoban levels.
/// </summary>
[CreateAssetMenu]
public class SokobanPuzzleLevel : ScriptableObject
{
    /// <summary>
    /// The size of our <see cref="SokobanPuzzleLevel"/> in a <see cref="Vector2Int"/>.
    /// </summary>
    public Vector2Int Size => size;

    /// <summary>
    /// Indicates whether the tilemap has been built.
    /// </summary>
    public bool HasGeneratedTiles => tiles != null;

    [SerializeField]
    private SokobanTileType[] tiles;

    [SerializeField]
    private Vector2Int size;

    /// <summary>
    /// Builds an empty tilemap.
    /// </summary>
    public void GenerateTiles(Vector2Int size, SokobanTileType defaulType = SokobanTileType.Floor)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        serializedObject.FindProperty("size").vector2IntValue = size;

        serializedObject.FindProperty("tiles").arraySize = size.x * size.y;
        serializedObject.ApplyModifiedProperties();

        // Initialize each tile
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                SetTileTypeAt(x, y, defaulType);
            }
        }

    }

    /// <summary>
    /// Get a tile at a specified coordinate.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">Thrown if the coordinate is out of range.</exception>
    public SokobanTileType GetTileTypeAt(int x, int y)
    {
        if (IsOutOfRange(x, y))
        {
            throw new IndexOutOfRangeException();
        }

        return tiles[y * Size.x + x];
    }

    /// <summary>
    /// Set a tile at a specified coordinate.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">Thrown if the coordinate is out of range.</exception>
    public void SetTileTypeAt(int x, int y, SokobanTileType value)
    {
        if (IsOutOfRange(x, y))
        {
            throw new IndexOutOfRangeException();
        }

        tiles[y * Size.x + x] = value;
    }

    /// <summary>
    /// Checks if the specified position is out-of-range range of the tile array.
    /// </summary>
    private bool IsOutOfRange(int x, int y) => x < 0 || x >= Size.x || y < 0 || y >= Size.y;
}
