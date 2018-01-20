/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/20/2018
 * Description: The top-level manager for a sokoban puzzle.
 */

using UnityEngine;

/// <inheritdoc />
/// <summary>
/// The top-level manager for a sokoban puzzle.
/// </summary>
[ExecuteInEditMode]
public class SokobanPuzzleRoom : Room
{
    /// <summary>
    /// The transformation matrix for the grid.
    /// The view matrix has a fixed rotation based on the orientation (topdown) of the grid.
    /// The position of the matrix is inherited from this <see cref="Transform"/>.
    /// </summary>
    public Matrix4x4 GridViewMatrix 
    {
        get
        {
            Vector3 rotation = Vector3.zero;
            Vector3 position = transform.position;

            // Rotate our grid so it faces up.
            rotation.x = 90;

            // Flip the y and z axis as we are rotated to face up (topdown).
            float oldY = position.y;
            position.y = position.z;
            position.z = -oldY;

            return Matrix4x4.Rotate(Quaternion.Euler(rotation)) * Matrix4x4.Translate(position);
        }
    }

    /// <summary>
    /// The puzzle level which this <see cref="SokobanPuzzleRoom"/> will create.
    /// </summary>
    public SokobanPuzzleLevel Level => level;

    [SerializeField]
    private SokobanPuzzleLevel level;

    protected override void Start()
    {
        base.Start();
        if (level)
        {
            Size = level.Size;
        }
    }
}
