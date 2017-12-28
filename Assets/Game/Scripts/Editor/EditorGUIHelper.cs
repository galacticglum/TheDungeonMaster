/*
 * Author: Shon Verch
 * File Name: EditorGUIHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Extension functionality related to the Editor GUI.
 */

using UnityEditor;
using UnityEngine;

/// <summary>
/// Extension functionality related to the Editor GUI.
/// </summary>
public static class EditorGUIHelper
{
    /// <summary>
    /// Draws a header in the inspector.
    /// </summary>
    /// <param name="content">The content of the header.</param>
    private static void DrawHeader(string content) => DrawHeader(new GUIContent(content));

    /// <summary>
    /// Draws a header in the inspector.
    /// </summary>
    /// <param name="content">The content of the header.</param>
    private static void DrawHeader(GUIContent content)
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(content, EditorStyles.boldLabel);
    }
}