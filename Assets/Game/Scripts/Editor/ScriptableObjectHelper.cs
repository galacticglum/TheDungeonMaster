/*
 * Author: Shon Verch
 * File Name: ScriptableObjectHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/08/18
 * Modified Date: 01/08/18
 * Description: Extensions functionality regard scriptable objects.
 */
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Extensions functionality regard scriptable objects.
/// </summary>
public static class ScriptableObjectHelper
{
    /// <summary>
    ///	Creates a unique <see cref="ScriptableObject"/> of type <typeparamref name="T"/> at the specified file path.
    /// </summary>
    public static T CreateAsset<T>(string filePath) where T : ScriptableObject
    {
        // If we don't have a file path provided or the directory is invalid.
        if (filePath == string.Empty || !Directory.Exists(Path.GetDirectoryName(filePath))) return default(T);

        T asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return asset;
    }
}