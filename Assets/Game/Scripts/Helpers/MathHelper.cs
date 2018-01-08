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

    public static int FloorToEven(this int value) => value % 2 == 0 ? value : value - 1;
    public static int CeilToEven(this int value) => value % 2 == 0 ? value : value + 1;

    public static int FloorToEven(this float value) => FloorToEven(Mathf.FloorToInt(value));
    public static int CeilToEven(this float value) => CeilToEven(Mathf.CeilToInt(value));

    public static Vector2 FloorToEven(this Vector2 vector) => new Vector2(vector.x.FloorToEven(), vector.y.FloorToEven());
    public static Vector2 CeilToEven(this Vector2 vector) => new Vector2(vector.x.CeilToEven(), vector.y.CeilToEven());

    public static Vector3 FloorToEven(this Vector3 vector) => new Vector3(vector.x.FloorToEven(), vector.y.FloorToEven(), vector.z.FloorToEven());
    public static Vector3 CeilToEven(this Vector3 vector) => new Vector3(vector.x.CeilToEven(), vector.y.CeilToEven(), vector.z.CeilToEven());
}