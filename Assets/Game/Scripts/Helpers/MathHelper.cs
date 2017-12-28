/*
 * Author: Shon Verch
 * File Name: MathHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Extension functions related to mathematics.
 */

using UnityEngine;

/// <summary>
/// Extension functions related to mathematics.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Compares whether two <see cref="Vector2"/> values are approximately equal.
    /// </summary>
    public static bool IsApproximatelyEqual(Vector2 a, Vector2 b) => Mathf.Approximately(a.x, a.y) &&
                                                                     Mathf.Approximately(a.y, b.y);

    /// <summary>
    /// Compares whether two <see cref="Vector3"/> values are approximately equal.
    /// </summary>
    public static bool IsApproximatelyEqual(Vector3 a, Vector3 b) => Mathf.Approximately(a.x, a.y) &&
                                                                     Mathf.Approximately(a.y, b.y) &&
                                                                     Mathf.Approximately(a.y, b.y);
}