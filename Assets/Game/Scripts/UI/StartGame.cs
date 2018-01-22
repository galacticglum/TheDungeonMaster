/*
 * Author: Shon Verch
 * File Name: StartGame.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Starts the game.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Starts the game.
/// </summary>
public class StartGame : MonoBehaviour
{
    /// <summary>
    /// Loads the main game scene.
    /// </summary>
    public void Execute() => SceneManager.LoadSceneAsync(1);
}
