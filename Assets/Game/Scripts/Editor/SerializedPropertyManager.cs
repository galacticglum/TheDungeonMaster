/*
 * Author: Shon Verch
 * File Name: SerializedPropertyManager.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: Manages all the serialized properties of a serialized object. Allows for quick dictionary-like access to serialized properties.
 */

using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Manages all the <see cref="SerializedProperty"/> values of a <see cref="SerializedObject"/>. 
/// Allows for quick dictionary-esque access to <see cref="SerializedProperty"/> values.
/// </summary>
public class SerializedPropertyManager
{
    /// <summary>
    /// The target <see cref="SerializedObject"/> which this <see cref="SerializedPropertyManager"/> operates on.
    /// </summary>
    public SerializedObject Target { get; }

    /// <summary>
    /// Gets a <see cref="SerializedProperty"/> by name.
    /// </summary>
    public SerializedProperty this[string name] => Get(name);

    /// <summary>
    /// The <see cref="SerializedProperty"/> cache for this <see cref="SerializedPropertyManager"/>.
    /// It stores all <see cref="SerializedProperty"/> values linked to their name.
    /// </summary>
    private readonly Dictionary<string, SerializedProperty> properties;

    /// <summary>
    /// Initialize this <see cref="SerializedPropertyManager"/>.
    /// </summary>
    public SerializedPropertyManager(SerializedObject target)
    {
        Target = target;
        properties = new Dictionary<string, SerializedProperty>();
    }

    /// <summary>
    /// Get a <see cref="SerializedProperty"/> by name.
    /// </summary>
    public SerializedProperty Get(string name)
    {
        if (!properties.ContainsKey(name))
        {
            properties[name] = Target.FindProperty(name);
        }

        return properties[name];
    }
}
