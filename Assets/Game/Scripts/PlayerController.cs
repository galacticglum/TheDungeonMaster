/*
 * Author: Shon Verch
 * File Name: PlayerController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/21/2017
 * Description: The top-level manager for the player character.
 */

using System;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;

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
[RequireComponent(typeof(vThirdPersonController))]
public class PlayerController : ControllerBehaviour
{
    /// <summary>
    /// This event is raised when the position of this <see cref="PlayerController"/> is changed.
    /// </summary>
    public event PlayerControllerEventHandler PositionChanged;

    /// <summary>
    /// Raise the player-position-changed event.
    /// </summary>
    private void OnPositionChanged() => PositionChanged?.Invoke(this, new PlayerControllerEventArgs(this));
    /// <summary>
    /// This event is raised when the walking animation triggers a footstep.
    /// </summary>
    public event PlayerControllerEventHandler FootstepTriggered;

    /// <summary>
    /// Raise the footstep-triggered event.
    /// </summary>
    public void OnFootstepTriggered() => FootstepTriggered?.Invoke(this, new PlayerControllerEventArgs(this));

    /// <summary>
    /// Determines whether the player can move.
    /// </summary>
    public bool CanMove
    {
        get { return canMove; }
        set
        {
            if (value == canMove) return;
            canMove = value;

            if (!canMove)
            {
                ThirdPersonController.input = Vector3.zero;
            }

            ThirdPersonController.lockMovement = !canMove;
        }
    }

    /// <summary>
    /// Determines whether the player is moving currently.
    /// </summary>
    public bool IsMoving => ThirdPersonController.speed != 0;

    /// <summary>
    /// The cards which belong to the player.
    /// </summary>
    public CardDeck Deck { get; private set; }

    /// <summary>
    /// The character motor for the player.
    /// </summary>
    public vThirdPersonController ThirdPersonController { get; private set; }

    /// <summary>
    /// Internal "tracking" variable for CanMove.
    /// </summary>
    private bool canMove = true;

    [SerializeField]
    private List<Card> startupDeck;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        ThirdPersonController = GetComponent<vThirdPersonController>();
        OnPositionChanged();

        Deck = new CardDeck(startupDeck.ToArray());
        ControllerDatabase.Get<GameController>().PauseStateChanged += (sender, args) => CanMove = args.WasPaused;
    }

    /// <summary>
    /// Called every fixed frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (!IsMoving) return;
        OnPositionChanged();
    }
}
