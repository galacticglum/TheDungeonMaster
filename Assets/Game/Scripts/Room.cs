/*
 * Author: Shon Verch
 * File Name: Room.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/20/2018
 * Description: A rectangular area.
 */

using System;
using UnityEngine;

/// <summary>
/// The event args pertaining to the <see cref="RoomEventHandler"/>.
/// </summary>
public class RoomEventArgs : EventArgs
{
    /// <summary>
    /// The room which this event happened.
    /// </summary>
    public Room Room { get; }

    /// <summary>
    /// Initializes the <see cref="RoomEventArgs"/>.
    /// </summary>
    public RoomEventArgs(Room room)
    {
        Room = room;
    }
}

/// <summary>
/// Basic event handler for all rooms.
/// </summary>
/// <param name="sender">The object which dispatched the event.</param>
/// <param name="args">The event args pertaining to the event.</param>
public delegate void RoomEventHandler(object sender, RoomEventArgs args);

/// <summary>
/// A rectangular area.
/// </summary>
public class Room : MonoBehaviour
{
    /// <summary>
    /// Indicates whether this room is complete.
    /// This value is true by default.
    /// </summary>
    public bool IsComplete { get; protected set; } = true;

    /// <summary>
    /// The centre point of this room.
    /// </summary>
    public Vector3 Centre => transform.position;

    /// <summary>
    /// This event is raised when the player enters this <see cref="Room"/>.
    /// </summary>
    public event RoomEventHandler PlayerEntered;

    /// <summary>
    /// This event is raised when the player exits this <see cref="Room"/>.
    /// </summary>
    public event RoomEventHandler PlayerExited;

    /// <summary>
    /// The size (of the bounds) of this room.
    /// </summary>
    public Vector2Int Size
    {
        get { return size; }
        protected set { size = value; }
    }

    [HideInInspector]
    [SerializeField]
    private Vector2Int size = new Vector2Int(1, 1);

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    protected virtual void Start()
    {
        ControllerDatabase.Get<RoomController>().RoomManager.Add(this);
    }

    /// <summary>
    /// Raise the PlayerEntered event.
    /// </summary>
    public void OnPlayerEnter()
    {
        PlayerEntered?.Invoke(this, new RoomEventArgs(this));
        OnPlayerEntered();
    }

    /// <summary>
    /// Raise the PlayerExited event.
    /// </summary>
    public void OnPlayerExit()
    {
        PlayerExited?.Invoke(this, new RoomEventArgs(this));
        OnPlayerExited();
    }

    /// <summary>
    /// Determines whether the specified position is inside this <see cref="Room"/>.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool ContainsPosition(Vector3 position) => position.x >= Centre.x - Size.x / 2f &&
                                              position.x <= Centre.x + Size.x / 2f &&
                                              position.z >= Centre.z - Size.y / 2f &&
                                              position.z <= Centre.z + Size.y / 2f;

    /// <summary>
    /// Called when the player enters this <see cref="Room"/>.
    /// </summary>
    protected virtual void OnPlayerEntered() {}

    /// <summary>
    /// Called when the player exits this <see cref="Room"/>.
    /// </summary>
    protected virtual void OnPlayerExited() {}
}