/*
 * Author: Shon Verch
 * File Name: SokobanTile.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: A single tile of a sokoban puzzle.
 */

using UnityEngine;

/// <summary>
/// A single tile of a sokoban puzzle.
/// </summary>
public class SokobanTile
{
    /// <summary>
    /// The type of this <see cref="SokobanTile"/>.
    /// </summary>
    public SokobanTileType Type { get; }

    /// <summary>
    /// The centre position of this <see cref="SokobanTile"/> in the world.
    /// </summary>
    public Vector2 Position { get; }

    /// <summary>
    /// Initialize a <see cref="SokobanTile"/>.
    /// </summary>
    public SokobanTile(SokobanTileType type, Vector2 position)
    {
        Type = type;
        Position = position;
    }

    /// <summary>
    /// Determines whether two <see cref="SokobanTile"/> values are neighbours.
    /// </summary>
    public bool IsNeighbourOf(SokobanTile target) => Vector2.Distance(Position, target.Position) == 1;
}