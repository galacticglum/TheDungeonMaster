/*
 * Author: Shon Verch
 * File Name: SokobanTileType.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: The type of a tile in a SokobanPuzzleLevel.
 */

/// <summary>
/// The type of a tile in a <see cref="SokobanPuzzleLevel"/>
/// </summary>
public enum SokobanTileType
{
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