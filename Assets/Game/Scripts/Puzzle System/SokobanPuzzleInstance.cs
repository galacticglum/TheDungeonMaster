/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/09/2018
 * Description: The top-level manager for a sokoban puzzle.
 */

using System;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// The top-level manager for a sokoban puzzle.
/// </summary>
[ExecuteInEditMode]
public class SokobanPuzzleInstance : MonoBehaviour
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

            // If out grid is not topdown, we don't need to do any rotation operations.
            if (!topDownGrid)
            {
                return Matrix4x4.Rotate(Quaternion.Euler(rotation)) * Matrix4x4.Translate(position);
            }

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
    /// The offset of the grid in a <see cref="Vector2"/>.
    /// The y-axis is determined by whether this <see cref="SokobanPuzzleInstance"/> is topdown. 
    /// If it is topdown, the y-axis is actually our z-axis.
    /// </summary>
    public Vector2 Offset => topDownGrid ? new Vector2(transform.position.x, transform.position.z) : new Vector2(transform.position.x, transform.position.y);

    /// <summary>
    /// The size of the of this grid rounded to the nearest integer.
    /// </summary>
    public Vector2Int RoundedSize => new Vector2Int(size.x.FloorToEven(), size.y.FloorToEven());

    /// <summary>
    /// The puzzle level which this <see cref="SokobanPuzzleInstance"/> will create.
    /// </summary>
    public SokobanPuzzleLevel Level => level;

    [Tooltip("Determines the orientation of the grid.")]
    [SerializeField]
    private bool topDownGrid = true;

    [SerializeField]
    private SokobanPuzzleLevel level;
    
    /// <summary>
    /// The size of this grid.
    /// </summary>
    [SerializeField]
    private Vector2 size;

    private void Reset()
    {
        size = new Vector2(2, 2);
        topDownGrid = true;
    }
}
