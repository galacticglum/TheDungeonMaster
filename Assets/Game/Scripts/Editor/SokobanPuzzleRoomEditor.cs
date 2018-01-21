﻿/*
 * Author: Shon Verch
 * File Name: SokobanPuzzleInstanceEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/07/2018
 * Modified Date: 01/20/2018
 * Description: Custom editor for a SokobanPuzzleInstance.
 */

using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Custom editor for a <see cref="SokobanPuzzleRoom"/>.
/// </summary>
[CustomEditor(typeof(SokobanPuzzleRoom))]
public class SokobanPuzzleRoomEditor : Editor
{
    private static GUIContent EditModeButton => EditorGUIUtility.IconContent("EditCollider");

    private SokobanPuzzleRoom sokobanPuzzleRoom;
    private SerializedPropertyManager propertyManager;
    private BoxBoundsHandle boxBoundsHandle;

    private bool showGeneralSettings = true;
    private bool showEditTileSettings;
    private bool showLevelSettings;

    private bool isEditingBounds;
    private bool isTileSelected;

    private Vector2Int selectedTileIndex;

    private void OnEnable()
    {
        propertyManager = new SerializedPropertyManager(serializedObject);
        boxBoundsHandle = new BoxBoundsHandle
        {
            axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y
        };

        EditMode.onEditModeStartDelegate += (editor, mode) => isEditingBounds = true;
        EditMode.onEditModeEndDelegate += editor => isEditingBounds = false;

        sokobanPuzzleRoom = (SokobanPuzzleRoom)target;

        selectedTileIndex = Vector2Int.zero;
        showEditTileSettings = false;
        isTileSelected = false;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propertyManager["level"], new GUIContent("Level"));
        if (EditorGUI.EndChangeCheck())
        {
            LevelAssetUpdated();
        }

        GUIContent createButtonContent = new GUIContent("Create");
        if (GUILayout.Button(createButtonContent, GUILayout.Width(GUI.skin.button.CalcSize(createButtonContent).x + 2)))
        {
            string createAssetFilePath = EditorUtility.SaveFilePanelInProject("Create Sokoban Level Asset", "level", "asset", "Please enter a filename to create the level to");

            SokobanPuzzleLevel levelAsset = ScriptableObjectHelper.CreateAsset<SokobanPuzzleLevel>(createAssetFilePath);
            if (levelAsset != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Changed level asset of sokoban puzzle instance.");
                propertyManager["level"].objectReferenceValue = levelAsset;
                LevelAssetUpdated();
            }
        }
        
        EditorGUILayout.EndHorizontal();

        if (propertyManager["level"].objectReferenceValue != null)
        {
            EditorGUIHelper.Splitter();

            if (GUILayout.Button("Load Level From File"))
            {
                string levelFilePath = EditorUtility.OpenFilePanel("Select Level File", Application.dataPath, "png");
                sokobanPuzzleRoom.Level.LoadFromFile(levelFilePath);
                LevelAssetUpdated();
            }

            showGeneralSettings = EditorGUIHelper.DrawSectionBox(new GUIContent("General Settings"), showGeneralSettings, () =>
            {
                EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Bounds", EditModeButton, () => new Bounds(boxBoundsHandle.center, boxBoundsHandle.size), this);

                EditorGUI.BeginChangeCheck();
                propertyManager["size"].vector2IntValue = EditorGUILayout.Vector2IntField(new GUIContent("Size"),
                    propertyManager["size"].vector2IntValue);
                if (EditorGUI.EndChangeCheck())
                {
                    Vector2Int vector = propertyManager["size"].vector2IntValue;
                    vector.x = Mathf.Max(2, vector.x);
                    vector.y = Mathf.Max(2, vector.y);

                    Undo.RecordObject(serializedObject.targetObject, "Changed size of sokoban puzzle instance");
                    propertyManager["size"].vector2IntValue = vector;
                }

                GUI.enabled = false;
                EditorGUILayout.Vector3Field(new GUIContent("Centre"), sokobanPuzzleRoom.Centre);
                GUI.enabled = true;

                EditorGUILayout.BeginHorizontal();
                Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Generate Tile Map"), GUI.skin.button);

                GUI.enabled = !isEditingBounds;
                if (GUI.Button(buttonRect, new GUIContent("Generate Tile Map", "Create the tile map.")))
                {
                    if (EditorUtility.DisplayDialog("Generate Tile Map",
                        "Are you sure you want to regenerate the tile map? " +
                        "This will remove ALL modifications made to the level settings—including tile setup.", "Yes", "No"))
                    {
                        sokobanPuzzleRoom.Level.GenerateTiles(sokobanPuzzleRoom.Size);
                        LevelAssetUpdated();

                        showLevelSettings = true;
                        EditorGUIHelper.RemoveFocus();
                    }
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            });

            showLevelSettings = EditorGUIHelper.DrawSectionBox(new GUIContent("Level Settings"), showLevelSettings, () =>
            {
                EditorGUILayout.BeginHorizontal();
                Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Generate Level"), GUI.skin.button);
                if (GUI.Button(buttonRect, "Generate Level"))
                {
                    // TODO: Generate level phase
                }

                EditorGUILayout.EndHorizontal();
            });

            if (isTileSelected)
            {
                showEditTileSettings = EditorGUIHelper.DrawSectionBox(new GUIContent("Edit Selected Tile"), showEditTileSettings, () =>
                {
                    EditorGUILayout.BeginHorizontal();

                    GUI.enabled = false;
                    EditorGUILayout.LabelField($"Tile: ({selectedTileIndex.x}, {selectedTileIndex.y})");
                    GUI.enabled = true;

                    GUIContent closeButtonContent = new GUIContent("Close");
                    float closeButtonWidth = EditorStyles.miniButton.CalcSize(closeButtonContent).x + 1;

                    if (GUILayout.Button(closeButtonContent, GUILayout.Width(closeButtonWidth)))
                    {
                        isTileSelected = false;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUIHelper.Splitter();

                    int selectedX = selectedTileIndex.x;
                    int selectedY = selectedTileIndex.y;

                    SokobanPuzzleTile tile = sokobanPuzzleRoom.Level.GetTileAt(selectedX, selectedY);

                    tile.Type = (SokobanTileType)EditorGUILayout.EnumPopup("Type", tile.Type);
                    tile.SpawnCrate = EditorGUILayout.Toggle("Should Spawn Crate", tile.SpawnCrate);
                });
            }
        }

        propertyManager.Target.ApplyModifiedProperties();
        Undo.FlushUndoRecordObjects();

        AssetDatabase.SaveAssets();
    }

    private void LevelAssetUpdated()
    {
        if (propertyManager["level"].objectReferenceValue == null) return;

        propertyManager["size"].vector2IntValue = ((SokobanPuzzleLevel)propertyManager["level"].objectReferenceValue).Size;
        propertyManager.Target.ApplyModifiedProperties();

        AssetDatabase.SaveAssets();
    }

    private void OnSceneGUI()
    {
        if (sokobanPuzzleRoom.Level == null) return;
        using (new Handles.DrawingScope(sokobanPuzzleRoom.GridViewMatrix))
        {
            if (isEditingBounds)
            {
                boxBoundsHandle.center = Vector3.zero;
                boxBoundsHandle.size = (Vector2)propertyManager["size"].vector2IntValue;

                EditorGUI.BeginChangeCheck();
                boxBoundsHandle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject((SokobanPuzzleRoom)target,
                        $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");

                    Vector3Int handleSize = boxBoundsHandle.size.FloorToInt();
                    propertyManager["size"].vector2IntValue = new Vector2Int(handleSize.x, handleSize.y);
                }
            }

            Vector2Int roundedSize = sokobanPuzzleRoom.Size;

            int halfWidth = roundedSize.x / 2;
            int halfHeight = roundedSize.y / 2;

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

            if (!isEditingBounds && sokobanPuzzleRoom.Level.HasGeneratedTiles)
            {
                int tileMapHalfWidth = sokobanPuzzleRoom.Level.Size.x / 2;
                int tileMapHalfHeight = sokobanPuzzleRoom.Level.Size.y / 2;

                // Reset tint
                Handles.color = Color.white;
                for (int y = -tileMapHalfHeight; y < tileMapHalfHeight; y++)
                {
                    for (int x = -tileMapHalfWidth; x < tileMapHalfWidth; x++)
                    {
                        Vector2Int tileIndex = new Vector2Int(x + tileMapHalfWidth, y + tileMapHalfHeight);
                        Vector3 position = new Vector3(x, y, 0);

                        Color faceColour = GetTileColour(sokobanPuzzleRoom.Level.GetTileAt(tileIndex.x, tileIndex.y));
                        if (x < -halfWidth || x >= halfWidth || y < -halfHeight || y >= halfHeight)
                        {
                            faceColour = new Color(1, 0, 0, 0.05f);
                        }

                        // Draw tile
                        Handles.DrawSolidRectangleWithOutline(new Rect(position, Vector2.one), faceColour, Color.green);

                        SokobanPuzzleTile tile = sokobanPuzzleRoom.Level.GetTileAt(tileIndex.x, tileIndex.y);
                        if (tile.SpawnCrate)
                        {
                            Handles.Label(position, Resources.Load<Texture>("Images/GUI/wooden_crate"));
                        }

                        if (!Handles.Button(position + new Vector3(0.5f, 0.5f), Quaternion.identity, 0.125f, 0.125f,
                            Handles.DotHandleCap)) continue;

                        // Handle tile selection
                        selectedTileIndex = tileIndex;
                        showEditTileSettings = true;
                        isTileSelected = true;

                        Repaint();
                        EditorUtility.SetDirty(this);
                    }
                }
            }
        }

        propertyManager.Target.ApplyModifiedProperties();
    }

    private static void InitializeGizmoColour(int value) => Handles.color = value == 0 ? Color.white : Color.green;

    private static Color GetTileColour(SokobanPuzzleTile tile)
    {
        switch (tile.Type)
        {
            case SokobanTileType.Floor:
                return new Color(0.933f, 0.95f, 0.25f, 0.1f);
            case SokobanTileType.Wall:
                return new Color(0, 0, 1, 0.1f);
            case SokobanTileType.Goal:
                return new Color(0, 2, 0, 0.1f);
            default:
                throw new ArgumentOutOfRangeException(nameof(tile.Type), tile.Type, null);
        }
    }
}
