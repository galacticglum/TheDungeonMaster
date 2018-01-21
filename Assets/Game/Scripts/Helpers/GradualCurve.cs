/*
 * Author: Shon Verch
 * File Name: GradualCurve.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: A gradual sigmoid curve.
 */

using System;
using UnityEngine;

/// <summary>
/// A gradual sigmoid curve defined by 3t^2 - 2t^3
/// </summary>
public static class GradualCurve
{
    /// <summary>
    /// Interpolates between a and b using a sigmoid curve.
    /// </summary>
    public static float Interpolate(float a, float b, float t) => a + (b - a) * (float)(3 * Math.Pow(t, 2) - 2 * Math.Pow(t, 3));

    public static Quaternion Interpolate(Quaternion a, Quaternion b, float t) => Quaternion.Slerp(a, b, (float)(3 * Math.Pow(t, 2) - 2 * Math.Pow(t, 3)));

    /// <summary>
    /// Interpolates between a and b using a sigmoid curve.
    /// </summary>
    public static Vector2 Interpolate(Vector2 a, Vector2 b, float t) => new Vector2(Interpolate(a.x, b.x, t), Interpolate(a.y, b.y, t));

    /// <summary>
    /// Interpolates between a and b using a sigmoid curve.
    /// </summary>
    public static Vector3 Interpolate(Vector3 a, Vector3 b, float t) => new Vector3(Interpolate(a.x, b.x, t), Interpolate(a.y, b.y, t), Interpolate(a.z, b.z, t));
}