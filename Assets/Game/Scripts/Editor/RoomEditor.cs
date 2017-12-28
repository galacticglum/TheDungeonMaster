/*
 * Author: Shon Verch
 * File Name: RoomEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Room component inspector.
 */

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Room component inspector.
/// </summary>
[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    private static GUIContent EditModeButton => EditorGUIUtility.IconContent("EditCollider");
    private BoxBoundsHandle boxBoundsHandle;
    private bool canEditBounds;

    private void OnEnable()
    {
        boxBoundsHandle = new BoxBoundsHandle
        {
            axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y
        };

        EditMode.onEditModeStartDelegate += (editor, mode) => canEditBounds = true;
        EditMode.onEditModeEndDelegate += editor => canEditBounds = false;

        boxBoundsHandle.handleColor = Color.yellow;
        boxBoundsHandle.wireframeColor = Color.green;
    }

    public override void OnInspectorGUI()
    {
        EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Bounds", EditModeButton, () => new Bounds(boxBoundsHandle.center, boxBoundsHandle.size), this);

        SerializedProperty sizeProperty = serializedObject.FindProperty("size");
        sizeProperty.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Size"), sizeProperty.vector2Value);

        GUI.enabled = false;
        EditorGUILayout.Vector3Field(new GUIContent("Centre"), ((Room)target).transform.position);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Room room = (Room)target;
        SerializedProperty sizeProperty = serializedObject.FindProperty("size");

        using (new Handles.DrawingScope(GetGridViewMatrix(room.transform)))
        {
            if (canEditBounds)
            {
                boxBoundsHandle.size = sizeProperty.vector2Value;
                boxBoundsHandle.center = Vector3.zero;

                EditorGUI.BeginChangeCheck();
                boxBoundsHandle.DrawHandle();
                if (!EditorGUI.EndChangeCheck()) return;

                Undo.RecordObject((Room)target, $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");
                sizeProperty.vector2Value = boxBoundsHandle.size;

                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                Vector2 bottomLeft = new Vector2(-sizeProperty.vector2Value.x / 2f, -sizeProperty.vector2Value.y / 2f);
                Vector2 bottomRight = new Vector2(sizeProperty.vector2Value.x / 2f, -sizeProperty.vector2Value.y / 2f);

                Vector2 topLeft = new Vector2(-sizeProperty.vector2Value.x / 2f, sizeProperty.vector2Value.y / 2f);
                Vector2 topRight = new Vector2(sizeProperty.vector2Value.x / 2f, sizeProperty.vector2Value.y / 2f);

                Handles.DrawLine(bottomLeft, bottomRight);
                Handles.DrawLine(bottomLeft, topLeft);
                Handles.DrawLine(topLeft, topRight);
                Handles.DrawLine(topRight, bottomRight);
            }
        }
    }

    private static Matrix4x4 GetGridViewMatrix(Transform transform)
    {
        Vector3 rotation = new Vector3(90, 0, 0);
        Vector3 position = transform.position;

        return Matrix4x4.Translate(position) * Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(rotation));
    }
}