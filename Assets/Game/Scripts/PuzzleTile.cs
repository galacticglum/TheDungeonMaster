/*
 * Author: Shon Verch
 * File Name: PuzzleTile.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/20/2018
 * Description: An individual tile (component) of a PuzzleRoom.
 */

using UnityEngine;

/// <summary>
/// An individual tile (component) of a <see cref="PuzzleRoom"/>.
/// </summary>
public class PuzzleTile
{
    /// <summary>
    /// The type of this <see cref="PuzzleTile"/>
    /// </summary>
    public SokobanTileType Type { get; }

    /// <summary>
    /// The (centre) position  of this <see cref="PuzzleTile"/>.
    /// </summary>
    public Vector2Int Position { get; }

    /// <summary>
    /// Initialize a new <see cref="PuzzleTile"/>.
    /// </summary>
    /// <param name="type">The type of this <see cref="PuzzleTile"/>.</param>
    /// <param name="position">The position of this <see cref="PuzzleTile"/>.</param>
    public PuzzleTile(SokobanTileType type, Vector2Int position)
    {
        Type = type;
        Position = position;
    }

    /// <summary>
    /// Determins whether two <see cref="PuzzleTile"/> values are neighbours.
    /// </summary>
    /// <param name="other">The other <see cref="PuzzleTile"/> to compare with this <see cref="PuzzleTile"/>.</param>
    public bool IsNeighbourOf(PuzzleTile other) => Vector2Int.Distance(Position, other.Position) == 1;
}
