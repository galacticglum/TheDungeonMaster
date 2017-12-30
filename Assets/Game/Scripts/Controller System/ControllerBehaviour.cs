/*
 * Author: Shon Verch
 * File Name: ControllerBehaviour.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/29/2017
 * Modified Date: 12/29/2017
 * Description: The base class from which every controller derives from.
 */

using UnityEngine;

/// <summary>
/// The base class from which every controller derives from.
/// </summary>
[DisallowMultipleComponent]
public abstract class ControllerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Indicates whether this <see cref="ControllerBehaviour"/> should be destroyed.
    /// </summary>
    public bool ShouldDestroy { get; set; }

    /// <summary>
    /// Called every time this <see cref="ControllerBehaviour"/> is reset.
    /// If this method is overriden, its base MUST be called for the <see cref="ControllerBehaviour"/> to function properly.
    /// </summary>
    protected virtual void Reset()
    {
        if (FindObjectOfType<ControllerDatabase>() == null)
        {
            Debug.LogWarning("For controllers to function, you MUST place a ControllerDatabase component into the scene.");
        }

        // If our controller already exists in the database, DESTROY IT!
        ShouldDestroy = ControllerDatabase.Exists(GetType());
    }
}