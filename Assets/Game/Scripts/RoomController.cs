/*
 * Author: Shon Verch
 * File Name: RoomController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Manages all rooms and keeps track of the current room the player is in.
 */

using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// The event args pertaining to the <see cref="CurrentRoomChangedEventHandler"/>.
/// </summary>
public class CurrentRoomChangedEventArgs : EventArgs
{
    /// <summary>
    /// The old room which the player was in.
    /// </summary>
    public Room OldRoom { get; }

    /// <summary>
    /// The new room which the player is in.
    /// </summary>
    public Room NewRoom { get; }

    /// <summary>
    /// Initialize a <see cref="CurrentRoomChangedEventArgs"/>.
    /// </summary>
    /// <param name="oldRoom">The old room which the player was in.</param>
    /// <param name="newRoom">The new room which the player is in.</param>
    public CurrentRoomChangedEventArgs(Room oldRoom, Room newRoom)
    {
        OldRoom = oldRoom;
        NewRoom = newRoom;
    }
}

/// <summary>
/// Event handler for when the player changes rooms.
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void CurrentRoomChangedEventHandler(object sender, CurrentRoomChangedEventArgs args);

/// <summary>
/// Manages all rooms and keeps track of the current room the player is in.
/// </summary>
public class RoomController : MonoBehaviour
{
    /// <summary>
    /// This event is raised when the current room changes.
    /// </summary>
    public event CurrentRoomChangedEventHandler CurrentRoomChanged;

    /// <summary>
    /// Raise the player-room-changed event.
    /// </summary>
    private void OnCurrentRoomChanged(Room oldRoom, Room newRoom) => CurrentRoomChanged?.Invoke(this, new CurrentRoomChangedEventArgs(oldRoom, newRoom));

    /// <summary>
    /// The current room which the player is in.
    /// </summary>
    public Room CurrentRoom
    {
        get { return currentRoom; }
        set
        {
            if (value == currentRoom) return;

            Room oldRoom = currentRoom;
            currentRoom = value;

            OnCurrentRoomChanged(oldRoom, currentRoom);
        }
    }

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Room[] rooms;

    private Room currentRoom;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        playerController.PositionChangedEventHandler += OnPlayerPositionChanged;
    }

    /// <summary>
    /// The player position has changed.
    /// </summary>
    /// <param name="sender">The object which dispatched the event.</param>
    /// <param name="args">The event args pertaining to the event.</param>
    private void OnPlayerPositionChanged(object sender, PlayerControllerEventArgs args)
    {
        CurrentRoom = GetRoomFromPosition(args.PlayerController.transform.position);
    }

    /// <summary>
    /// Retrieves the room which the specified <see cref="Vector3"/> is in.
    /// </summary>
    public Room GetRoomFromPosition(Vector3 position)
    {
        return rooms.FirstOrDefault(room => position.x >= room.Centre.x - room.Size.x / 2f && 
                                            position.x <= room.Centre.x + room.Size.x / 2f && 
                                            position.z >= room.Centre.z - room.Size.y / 2f && 
                                            position.z <= room.Centre.z + room.Size.y / 2f);
    }
}
