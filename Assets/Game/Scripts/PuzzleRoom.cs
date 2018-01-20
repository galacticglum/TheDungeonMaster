/*
 * Author: Shon Verch
 * File Name: PuzzleRoom.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/20/2018
 * Modified Date: 01/20/2018
 * Description: The room which manages puzzle logic.
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The room which manages puzzle logic.
/// </summary>
public class PuzzleRoom : Room
{
    private PuzzleTile[,] tiles;
    private Dictionary<PuzzleTile, GameObject> tileGameObjects;

    private PlayerController playerController;

    protected override void Start()
    {
        base.Start();

        playerController = ControllerDatabase.Get<PlayerController>();
        tileGameObjects = new Dictionary<PuzzleTile, GameObject>();
        tiles = new PuzzleTile[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                tiles[x, y] = new PuzzleTile(SokobanTileType.Floor, new Vector2Int(x, y));
                GameObject cubeGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cubeGameObject.transform.position = Centre + new Vector3(Size.x / 2f - cubeGameObject.transform.localScale.x / 2f - x, cubeGameObject.transform.localScale.y / 2f, 
                                                        Size.y / 2f - cubeGameObject.transform.localScale.z / 2f - y);

                cubeGameObject.transform.SetParent(transform);
                Destroy(cubeGameObject.GetComponent<BoxCollider>());

                tileGameObjects.Add(tiles[x, y], cubeGameObject);
            }
        }
    }

    private void Update()
    {
        Vector3 playerRoomPosition = Centre - playerController.transform.position + new Vector3(Size.x / 2f, 0, Size.y / 2f);
        Vector2Int tilePosition = new Vector2Int(Mathf.FloorToInt(playerRoomPosition.x), Mathf.FloorToInt(playerRoomPosition.z));

        if (tilePosition.x < 0 || tilePosition.x >= Size.x || tilePosition.y < 0 || tilePosition.y >= Size.y) return;
        tileGameObjects[tiles[tilePosition.x, tilePosition.y]].GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
}