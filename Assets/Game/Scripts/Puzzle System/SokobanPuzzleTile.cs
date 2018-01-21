/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleTile.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/20/2018
 * Description: An individual tile (component) of a PuzzleRoom.
 */

using System;
using UnityEngine;

/// <summary>
/// An individual tile (component) of a <see cref="SokobanPuzzleRoom"/>.
/// </summary>
[Serializable]
public class SokobanPuzzleTile
{
    /// <summary>
    /// The type of this <see cref="SokobanPuzzleTile"/>
    /// </summary>
    public SokobanTileType Type
    {
        get { return type; }
        set { type = value; }
    }

    /// <summary>
    /// Indicates whether a crate should be spawned on this <see cref="SokobanPuzzleTile"/>.
    /// </summary>
    public bool SpawnCrate
    {
        get { return spawnCrate; }
        set { spawnCrate = value; }
    }

    [SerializeField]
    private SokobanTileType type;

    [SerializeField]
    private bool spawnCrate;

    /// <summary>
    /// Initialize a new <see cref="SokobanPuzzleTile"/>.
    /// </summary>
    /// <param name="type">The type of this <see cref="SokobanPuzzleTile"/>.</param>
    public SokobanPuzzleTile(SokobanTileType type)
    {
        this.type = type;
    }
}
