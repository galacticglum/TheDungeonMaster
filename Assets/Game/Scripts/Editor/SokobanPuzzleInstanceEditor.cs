/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleInstanceEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/07/2018
 * Description: Custom editor for a SokobanPuzzleInstance.
 */

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Custom editor for a <see cref="SokobanPuzzleInstance"/>.
/// </summary>
[CustomEditor(typeof(SokobanPuzzleInstance))]
public class SokobanPuzzleInstanceEditor : Editor
{
    private static GUIContent EditModeButton => EditorGUIUtility.IconContent("EditCollider");

    private SokobanPuzzleInstance sokobanPuzzleInstance;
    private SerializedPropertyManager propertyManager;
    private BoxBoundsHandle boxBoundsHandle;
    private bool isEditingBounds;

    private bool showGeneralSettings = true;
    private bool showLevelSettings;
    private bool hasSavedGeneralSettings;

    private void OnEnable()
    {
        propertyManager = new SerializedPropertyManager(serializedObject);
        boxBoundsHandle = new BoxBoundsHandle
        {
            axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y
        };

        EditMode.onEditModeStartDelegate += (editor, mode) => isEditingBounds = true;
        EditMode.onEditModeEndDelegate += editor => isEditingBounds = false;

        sokobanPuzzleInstance = (SokobanPuzzleInstance)target;
    }

    public override void OnInspectorGUI()
    {
        showGeneralSettings = EditorGUIHelper.DrawSectionBox(new GUIContent("General Settings"), showGeneralSettings, () =>
        {
            EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Bounds", EditModeButton, () => new Bounds(boxBoundsHandle.center, boxBoundsHandle.size), this);

            propertyManager["size"].vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Size"),
                propertyManager["size"].vector2Value);

            GUI.enabled = false;
            EditorGUILayout.Vector3Field(new GUIContent("Offset"), sokobanPuzzleInstance.transform.position);
            GUI.enabled = true;

            propertyManager["topDownGrid"].boolValue = EditorGUILayout.Toggle(new GUIContent("Is Topdown Grid"),
                propertyManager["topDownGrid"].boolValue);

            EditorGUILayout.BeginHorizontal();
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Generate Tile Map"), GUI.skin.button);

            GUI.enabled = !isEditingBounds;
            if (GUI.Button(buttonRect, new GUIContent("Generate Tile Map", "Create the tile map.")))
            {
                if (EditorUtility.DisplayDialog("Generate Tile Map",
                    "Are you sure you want to regenerate the tile map? " +
                    "This will remove ALL modifications made to the level settings—including tile setup.", "Yes", "No"))
                {
                    sokobanPuzzleInstance.InitializeTiles();
                    showLevelSettings = true;
                    hasSavedGeneralSettings = true;

                    EditorGUIHelper.RemoveFocus();
                }
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        });

        showLevelSettings = EditorGUIHelper.DrawSectionBox(new GUIContent("Level Settings"), showLevelSettings && hasSavedGeneralSettings, () =>
        {
            EditorGUILayout.BeginHorizontal();
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Generate Level"), GUI.skin.button);
            if (GUI.Button(buttonRect, "Generate Level"))
            {
                //sokobanPuzzleInstance.InitializeTiles();s
                showLevelSettings = true;
                hasSavedGeneralSettings = true;
            }
            EditorGUILayout.EndHorizontal();
        });

        propertyManager.Target.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        using (new Handles.DrawingScope(sokobanPuzzleInstance.GridViewMatrix))
        {
            if (isEditingBounds)
            {
                boxBoundsHandle.center = Vector3.zero;
                boxBoundsHandle.size = propertyManager["size"].vector2Value;

                EditorGUI.BeginChangeCheck();
                boxBoundsHandle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject((SokobanPuzzleInstance)target,
                        $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");

                    propertyManager["size"].vector2Value = boxBoundsHandle.size;
                }
            }
            else
            {
                Vector2 bottomLeft = new Vector2(-propertyManager["size"].vector2Value.x / 2f, -propertyManager["size"].vector2Value.y / 2f);
                Vector2 bottomRight = new Vector2(propertyManager["size"].vector2Value.x / 2f, -propertyManager["size"].vector2Value.y / 2f);

                Vector2 topLeft = new Vector2(-propertyManager["size"].vector2Value.x / 2f, propertyManager["size"].vector2Value.y / 2f);
                Vector2 topRight = new Vector2(propertyManager["size"].vector2Value.x / 2f, propertyManager["size"].vector2Value.y / 2f);

                Handles.DrawLine(bottomLeft, bottomRight);
                Handles.DrawLine(bottomLeft, topLeft);
                Handles.DrawLine(topLeft, topRight);
                Handles.DrawLine(topRight, bottomRight);
            }

            int halfWidth = sokobanPuzzleInstance.GridSize.x / 2;
            int halfHeight = sokobanPuzzleInstance.GridSize.y / 2;

            for (int x = -halfWidth; x < halfWidth + 1; x++)
            {
                InitializeGizmoColour(x);

                Vector3 a = new Vector3(x, -halfHeight, 0);
                Vector3 b = new Vector3(x, halfHeight, 0);

                Handles.DrawLine(a, b);
            }

            // draw the vertical lines
            for (int y = -halfHeight; y < halfHeight + 1; y++)
            {
                InitializeGizmoColour(y);

                Vector3 a = new Vector3(-halfWidth, y, 0);
                Vector3 b = new Vector3(halfWidth, y, 0);

                Handles.DrawLine(a, b);
            }

            if (!isEditingBounds)
            {
                int tileMapHalfWidth = sokobanPuzzleInstance.TileMapSize.x / 2;
                int tileMapHalfHeight = sokobanPuzzleInstance.TileMapSize.y / 2;

                // Reset tint
                Handles.color = Color.white;
                for (int y = -tileMapHalfHeight; y < tileMapHalfHeight; y++)
                {
                    for (int x = -tileMapHalfWidth; x < tileMapHalfWidth; x++)
                    {
                        Vector3 position = new Vector3(x, y, 0);

                        Color faceColour = new Color(0.933f, 0.95f, 0.25f, 0.1f);
                        if (x < -halfWidth || x >= halfWidth || y < -halfHeight || y >= halfHeight)
                        {
                            faceColour = new Color(1, 0, 0, 0.05f);
                        }

                        // Draw tile
                        Handles.DrawSolidRectangleWithOutline(new Rect(position, new Vector2(1, 1)), faceColour, Color.green);

                        //// Draw picker inner
                        //Handles.DrawSolidRectangleWithOutline(new Rect(position + new Vector3(0.375f, 0.375f), new Vector2(0.25f, 0.25f)), new Color(0, 1, 0, 0.2f), new Color(0, 0, 0, 1));

                        if (Handles.Button(position + new Vector3(0.5f, 0.5f), Quaternion.identity, 0.125f, 0.125f,
                            Handles.DotHandleCap))
                        {

                        }
                    }
                }
            }
        }

        propertyManager.Target.ApplyModifiedProperties();
    }

    private static void InitializeGizmoColour(int value) => Handles.color = value == 0 ? Color.white : Color.green;
}
