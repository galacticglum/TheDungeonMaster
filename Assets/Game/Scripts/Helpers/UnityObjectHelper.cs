/*
 * Author: Shon Verch
 * File Name: UnityObjectHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: Extension functionality related to Unity objects.
 */

using UnityEngine;

/// <summary>
/// Extension functionality related to Unity objects.
/// </summary>
public static class UnityObjectHelper
{
    /// <summary>
    /// Destroys all children belonging to the specified <see cref="Transform"/>.
    /// </summary>
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
}