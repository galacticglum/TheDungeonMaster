﻿/*
 * Author: Shon Verch
 * File Name: Room.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
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
    /// This event is raised when the player enters this <see cref="Room"/>.
    /// </summary>
    public event RoomEventHandler PlayerEntered;

    /// <summary>
    /// This event is raised when the player exits this <see cref="Room"/>.
    /// </summary>
    public event RoomEventHandler PlayerExited;

    /// <summary>
    /// The centre point of this room.
    /// </summary>
    public Vector3 Centre => transform.position;

    /// <summary>
    /// The size (of the bounds) of this room.
    /// </summary>
    public Vector2 Size => size;

    [SerializeField]
    private Vector2 size = Vector2.one;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        MasterDataController.Current.RoomController.RoomManager.Add(this);
    }

    /// <summary>
    /// Indicates whether this room is complete.
    /// </summary>
    public virtual bool IsComplete() => true;

    /// <summary>
    /// Raise the PlayerEntered event.
    /// </summary>
    public void OnPlayerEnter() => PlayerEntered?.Invoke(this, new RoomEventArgs(this));

    /// <summary>
    /// Raise the PlayerExited event.
    /// </summary>
    public void OnPlayerExit() => PlayerExited?.Invoke(this, new RoomEventArgs(this));
}