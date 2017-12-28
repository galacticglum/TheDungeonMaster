/*
 * Author: Shon Verch
 * File Name: RoomControllerEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: RoomControllerEditor custom inspector.
 */

using UnityEditor;
using UnityEngine;

/// <summary>
/// RoomControllerEditor custom inspector.
/// </summary>
[CustomEditor(typeof(RoomController))]
public class RoomControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty playerTransformProperty = serializedObject.FindProperty("playerController");
        EditorGUILayout.PropertyField(playerTransformProperty, new GUIContent("Player Controller"));

        EditorList.Show(serializedObject.FindProperty("rooms"), EditorListOptions.All);

        serializedObject.ApplyModifiedProperties();
    }
}