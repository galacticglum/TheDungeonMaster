/*
 * Author: Shon Verch
 * File Name: GameController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for global game states and data.
 */

using System;

/// <summary>
/// The event handler for when the pause state changes.
/// </summary>
public delegate void PauseStateChangedEventHandler(object sender, PauseStateEventArgs args);

/// <summary>
/// The event args for the pause state.
/// </summary>
public class PauseStateEventArgs : EventArgs
{
    /// <summary>
    /// The previous pause state.
    /// </summary>
    public bool WasPaused { get; }

    /// <summary>
    /// The current pause state.
    /// </summary>
    public bool IsPaused { get; }

    /// <summary>
    /// Initialize a new <see cref="PauseStateEventArgs"/>.
    /// </summary>
    public PauseStateEventArgs(bool wasPaused, bool isPaused)
    {
        WasPaused = wasPaused;
        IsPaused = isPaused;
    }
}

/// <summary>
/// The top-level manager for global game states and data.
/// </summary>
public class GameController : ControllerBehaviour
{
    /// <summary>
    /// Indicates whether the player has escaped the labyrinth.
    /// </summary>
    public bool HasWon { get; set; }

    /// <summary>
    /// Determines whether the game is paused.
    /// </summary>
    public bool IsPaused { get; private set; }
    
    /// <summary>
    /// Event which is raised when the pause state changes.
    /// </summary>
    public event PauseStateChangedEventHandler PauseStateChanged;

    /// <summary>
    /// Toggles between paused and unapused.
    /// </summary>
    public void TogglePause()
    {
        IsPaused = !IsPaused;
        PauseStateChanged?.Invoke(this, new PauseStateEventArgs(!IsPaused, IsPaused));
    }
}
