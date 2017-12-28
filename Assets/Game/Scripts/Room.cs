/*
 * Author: Shon Verch
 * File Name: Room.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: A rectangular area.
 */

using UnityEngine;

/// <summary>
/// A rectangular area.
/// </summary>
public class Room : MonoBehaviour
{
    public Vector3 Centre => transform.position;
    public Vector2 Size => size;

    [SerializeField]
    private Vector2 size = Vector2.one;
}