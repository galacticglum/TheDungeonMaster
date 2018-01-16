/*
 * Author: Shon Verch
 * File Name: ControllerDatabaseEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/29/2017
 * Modified Date: 1/15/2018
 * Description: Custom editor for the ControllerDatabase.
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor for the ControllerDatabase.
/// </summary>
[CustomEditor(typeof(ControllerDatabase))]
public class ControllerDatabaseEditor : Editor
{
    /// <summary>
    /// The visibility states of all the <see cref="ControllerBehaviour"/> elements.
    /// </summary>
    private Dictionary<string, bool> controllerVisibility;

    /// <summary>
    /// Called when the editor is enabled.
    /// </summary>
    private void OnEnable()
    {
        controllerVisibility = new Dictionary<string, bool>();
    }

    /// <summary>
    /// Draw the inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draw each controller behaviour.
        foreach (ControllerBehaviour controllerBehaviour in ControllerDatabase.GetEnumerable())
        {
            // Make the controller class name "friendly" by adding spaces and capitalizing each word (if needed).
            string controllerName = ObjectNames.NicifyVariableName(controllerBehaviour.GetType().Name);

            // If the controller visibility dictionary is NOT tracking this controller behaviour, add it to the visiblity dictionary under the default value: FALSE.
            if (!controllerVisibility.ContainsKey(controllerName))
            {
                controllerVisibility[controllerName] = false;
            }

            // Draw the controller behaviour section box.
            controllerVisibility[controllerName] = EditorGUIHelper.DrawSectionBox(new GUIContent(controllerName), controllerVisibility[controllerName], () =>
            {
                // This "Show" button will highlight the controller behaviour gameobject in the hierarchy when clicked.
                if (GUILayout.Button("Show", EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(new GUIContent("Show")).x)))
                {
                    EditorGUIUtility.PingObject(controllerBehaviour);
                }
            });
        }
    }
}