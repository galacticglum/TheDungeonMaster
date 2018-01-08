/*
 * Author: Shon Verch
 * File Name: SokobanTileType.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: The type of a SokobanTile.
 */

/// <summary>
/// The type of a <see cref="SokobanTile"/>.
/// </summary>
public enum SokobanTileType
{
    /// <summary>
    /// An empty tile which contains nothing.
    /// A tile with this type is void and has no visual representation.
    /// </summary>
    Empty,

    /// <summary>
    /// A floor which the player can stand on.
    /// </summary>
    Floor,

    /// <summary>
    /// A wall which blocks crates.
    /// </summary>
    Wall,

    /// <summary>
    /// A destination point for crates. 
    /// </summary>
    Goal
}