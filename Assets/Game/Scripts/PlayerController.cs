/*
 * Author: Shon Verch
 * File Name: PlayerController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/11/2017
 * Description: The top-level manager for the player character.
 */

using System;
using Invector.CharacterController;
using UnityEngine;
using Random = UnityEngine.Random;

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
                thirdPersonController.input = Vector3.zero;
            }

            thirdPersonController.lockMovement = !canMove;
        }
    }

    /// <summary>
    /// Determines whether the player is moving currently.
    /// </summary>
    public bool IsMoving => thirdPersonController.speed != 0;

    /// <summary>
    /// Internal "tracking" variable for CanMove.
    /// </summary>
    private bool canMove = true;
    private vThirdPersonController thirdPersonController;

    /// <summary>
    /// The cards which belong to the player.
    /// </summary>
    public CardDeck Deck { get; private set; }

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        thirdPersonController = GetComponent<vThirdPersonController>();
        OnPositionChanged();

        Deck = new CardDeck();
        Card[] cards = 
        {
            new Card("Punch",
                "Deals <color=#D5AB5CFF><i>6</i></color> damage to an enemy. ", 2),

            new Card("Power Punch",
                "Deals <color=#D5AB5CFF><i>6</i></color> damage to an enemy. " +
                "<color=#D5AB5CFF>Overcharge:</color> 3 cards.", 6, 0, 3),

            //new Card("Power Kick",
            //    "Your kick is mightier than the sun! Deals <color=#D5AB5CFF><i>10</i></color> damage to an enemy.", 4),

            //new Card("David", "The scrub", 4), 
            new Card("Jimmy the Scrawny", "Cis-gendered asian boi", 10, 0, 5), 
            new Card("Resurrection", "<color=#D5AB5CFF>Resurrects:</color> 1 cards.", 0, 1) 
        };

        // Initialize a test deck
        for (int i = 0; i < 25; i++)
        {
            Deck.Add(cards[Random.Range(0, cards.Length)].Clone());
        }

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
