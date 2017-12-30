/*
 * Author: Shon Verch
 * File Name: PlayerController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/29/2017
 * Description: Manages the various player functionality.
 */

using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// The event args pertaining to the <see cref="PlayerControllerEventHandler"/>.
/// </summary>
public class PlayerControllerEventArgs : EventArgs
{
    /// <summary>
    /// The player control of this event args.
    /// </summary>
    public PlayerController PlayerController { get; }

    /// <summary>
    /// Initialize a <see cref="PlayerControllerEventArgs"/> with a <see cref="global::PlayerController"/>.
    /// </summary>
    /// <param name="playerController">The player controller of this event args.</param>
    public PlayerControllerEventArgs(PlayerController playerController)
    {
        PlayerController = playerController;
    }
}

/// <summary>
/// Basic event handler for all player controllers.
/// </summary>
/// <param name="sender">The object which dispatched the event.</param>
/// <param name="args">The event args pertaining to the event.</param>
public delegate void PlayerControllerEventHandler(object sender, PlayerControllerEventArgs args);

/// <summary>
/// Manages the various player functionality.
/// </summary>
[RequireComponent(typeof(ThirdPersonUserControl))]
public class PlayerController : ControllerBehaviour
{
    /// <summary>
    /// This event is raised when the position of this <see cref="PlayerController"/> is changed.
    /// </summary>
    public event PlayerControllerEventHandler PositionChanged;

    private ThirdPersonUserControl thirdPersonUserControl;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        thirdPersonUserControl = GetComponent<ThirdPersonUserControl>();
        OnPositionChangedEventHandler();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (!thirdPersonUserControl.IsMoving) return;
        OnPositionChangedEventHandler();
    }

    /// <summary>
    /// Raise the player-position-changed event.
    /// </summary>
    private void OnPositionChangedEventHandler()
    {
        PlayerControllerEventArgs args = new PlayerControllerEventArgs(this);
        PositionChanged?.Invoke(this, args);
    }
}
