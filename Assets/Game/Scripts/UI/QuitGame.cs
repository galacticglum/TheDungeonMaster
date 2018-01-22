/*
 * Author: Shon Verch
 * File Name: QuitGame.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Quits the game.
 */

using UnityEngine;

/// <summary>
/// Quits the game.
/// </summary>
public class QuitGame : MonoBehaviour
{
    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Execute() => Application.Quit();
}
