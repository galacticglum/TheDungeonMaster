/*
 * Author: Shon Verch
 * File Name: UIManager.cs
 * Project Name: The Dungeon Master
 * Creation Date: 12/26/2017
 * Modified Date: 12/26/2017
 * Description: The manager for all top-level UI functionality and data.
 */

using UnityEngine;

/// <summary>
/// The manager for all top-level UI functionality and data.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// The active instance of this <see cref="UIManager"/>.
    /// </summary>
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Canvas canvas;

    /// <summary>
    /// The currently active UI canvas in the scene-graph.
    /// </summary>
    public Canvas Canvas => canvas;

    /// <summary>
    /// Called when this <see cref="UIManager"/> is enabled.
    /// </summary>
    private void OnEnable()
    {
        Instance = this;
    }
}
