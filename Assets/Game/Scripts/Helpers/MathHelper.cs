/*
 * Author: Shon Verch
 * File Name: MathHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/14/2018
 * Description: Extension functions related to mathematics.
 */

using UnityEngine;

/// <summary>
/// Extension functions related to mathematics.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Returns the largest even integer smaller to or equal to <paramref name="value"/>.
    /// </summary>
    public static int FloorToEven(this int value) => value % 2 == 0 ? value : value - 1;

    /// <summary>
    /// Returns the smallest even integer greater to or equal to <paramref name="value"/>.
    /// </summary>
    public static int CeilToEven(this int value) => value % 2 == 0 ? value : value + 1;

    /// <summary>
    /// Returns the largest even integer smaller to or equal to <paramref name="value"/>.
    /// </summary>
    public static int FloorToEven(this float value) => FloorToEven(Mathf.FloorToInt(value));

    /// <summary>
    /// Returns the smallest even integer greater to or equal to <paramref name="value"/>.
    /// </summary>
    public static int CeilToEven(this float value) => CeilToEven(Mathf.CeilToInt(value));

    /// <summary>
    /// Returns a <see cref="Vector2"/> where it's components (x, y) are the largest even 
    /// integers smaller to or equal to the respective component in <paramref name="vector"/>.
    /// </summary>
    public static Vector2 FloorToEven(this Vector2 vector) => new Vector2(vector.x.FloorToEven(), vector.y.FloorToEven());

    /// <summary>
    /// Returns a <see cref="Vector2"/> where it's components (x, y) are the smallest even 
    /// integers greater to or equal to the respective component in <paramref name="vector"/>.
    /// </summary>
    public static Vector2 CeilToEven(this Vector2 vector) => new Vector2(vector.x.CeilToEven(), vector.y.CeilToEven());

    /// <summary>
    /// Returns a <see cref="Vector3"/> where it's components (x, y, z) are the largest even 
    /// integers smaller to or equal to the respective component in <paramref name="vector"/>.
    /// </summary>
    public static Vector3 FloorToEven(this Vector3 vector) => new Vector3(vector.x.FloorToEven(), vector.y.FloorToEven(), vector.z.FloorToEven());

    /// <summary>
    /// Returns a <see cref="Vector3"/> where it's components (x, y, z) are the smallest even 
    /// integers greater to or equal to the respective component in <paramref name="vector"/>.
    /// </summary>
    public static Vector3 CeilToEven(this Vector3 vector) => new Vector3(vector.x.CeilToEven(), vector.y.CeilToEven(), vector.z.CeilToEven());
}