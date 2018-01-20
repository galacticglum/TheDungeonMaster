/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleTile.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/20/2018
 * Description: An individual tile (component) of a PuzzleRoom.
 */

using UnityEngine;

/// <summary>
/// An individual tile (component) of a <see cref="SokobanPuzzleRoom"/>.
/// </summary>
public class SokobanPuzzleTile
{
    /// <summary>
    /// The type of this <see cref="SokobanPuzzleTile"/>
    /// </summary>
    public SokobanTileType Type { get; }

    /// <summary>
    /// The (centre) position  of this <see cref="SokobanPuzzleTile"/>.
    /// </summary>
    public Vector2Int Position { get; }

    /// <summary>
    /// Initialize a new <see cref="SokobanPuzzleTile"/>.
    /// </summary>
    /// <param name="type">The type of this <see cref="SokobanPuzzleTile"/>.</param>
    /// <param name="position">The position of this <see cref="SokobanPuzzleTile"/>.</param>
    public SokobanPuzzleTile(SokobanTileType type, Vector2Int position)
    {
        Type = type;
        Position = position;
    }

    /// <summary>
    /// Determins whether two <see cref="SokobanPuzzleTile"/> values are neighbours.
    /// </summary>
    /// <param name="other">The other <see cref="SokobanPuzzleTile"/> to compare with this <see cref="SokobanPuzzleTile"/>.</param>
    public bool IsNeighbourOf(SokobanPuzzleTile other) => Vector2Int.Distance(Position, other.Position) == 1;
}
