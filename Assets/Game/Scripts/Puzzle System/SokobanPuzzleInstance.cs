/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: The top-level manager for a sokoban puzzle.
 */

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
    /// The actual size of our grid (rounded) in a <see cref="Vector2Int"/>.
    /// </summary>
    public Vector2Int GridSize => new Vector2Int(Mathf.FloorToInt(size.x), Mathf.FloorToInt(size.y));

    /// <summary>
    /// The size of our tilemap in a <see cref="Vector2Int"/>.
    /// </summary>
    public Vector2Int TileMapSize => new Vector2Int(tiles?.GetLength(0) ?? 0, tiles?.GetLength(1) ?? 0);

    /// <summary>
    /// The full size of our grid.
    /// </summary>
    [SerializeField]
    private Vector2 size = new Vector2(2, 2);

    /// <summary>
    /// Determines whether this <see cref="SokobanPuzzleInstance"/> faces up.
    /// </summary>
    [SerializeField]
    private bool topDownGrid = true;

    private SokobanTile[,] tiles;

    private void Reset()
    {
        tiles = null;
        size = new Vector2(2, 2);
        topDownGrid = true;
    }

    public void InitializeTiles()
    {
        size = size.CeilToEven();
        tiles = new SokobanTile[GridSize.x, GridSize.y];

        Transform tileParent = new GameObject("__LEVEL__").transform;
        tileParent.SetParent(transform);
        
        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                Vector2 position = new Vector2(GridSize.x / 2 - x + Offset.x - 0.5f, GridSize.y / 2 - y + Offset.y - 0.5f);
                tiles[x, y] = new SokobanTile(SokobanTileType.Empty, position);
                    
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                //Vector3 spawnPosition = new Vector3(position.x, position.y, 0);
                //if (topDownGrid)
                //{
                //    spawnPosition = new Vector3(position.x, transform.position.y, position.y);
                //}

                //cube.transform.position = spawnPosition;
                //cube.transform.SetParent(tileParent);
            }
        }
    }
}
