/*
 * Author: Shon Verch
 * File Name: UIManager.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/26/2017
 * Description: The manager for all top-level UI functionality and data.
 */

using UnityEngine;

/// <summary>
/// The manager for all top-level UI functionality and data.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class UIController : ControllerBehaviour
{
    /// <summary>
    /// The currently active UI canvas in the scene-graph.
    /// </summary>
    public Canvas Canvas { get; private set; }

    /// <summary>
    /// Called when this <see cref="UIController"/> is enabled.
    /// </summary>
    private void OnEnable()
    {
        Canvas = GetComponent<Canvas>();
    }
}
