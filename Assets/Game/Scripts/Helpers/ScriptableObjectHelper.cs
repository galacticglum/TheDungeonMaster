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
    public static void CreateAsset<T>(string filePath) where T : ScriptableObject
    {
        if (filePath == string.Empty) return;

        T asset = ScriptableObject.CreateInstance<T>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(filePath);

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        //Selection.activeObject = asset;
    }
}