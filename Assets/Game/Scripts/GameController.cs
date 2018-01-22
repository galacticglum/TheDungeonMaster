/*
 * Author: Shon Verch
 * File Name: GameController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: The top-level manager for global game states and data.
 */

using System;
using UnityEngine;

public delegate void PauseStateChangedEventHandler(object sender, PauseStateEventArgs args);
public class PauseStateEventArgs : EventArgs
{
    public bool WasPaused { get; }
    public bool IsPaused { get; }

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
