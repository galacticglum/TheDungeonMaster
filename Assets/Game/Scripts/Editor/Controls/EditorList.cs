/*
 * Author: Shon Verch
 * File Name: EditorList.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: A custom list control for the Editor.
 * URL: http://catlikecoding.com/unity/tutorials/editor/custom-list/
 */

using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom list control for the Editor.
/// </summary>
public static class EditorList
{
    private static readonly GUILayoutOption buttonWidth;
    private static readonly GUIContent moveButtonContent;
    private static readonly GUIContent duplicateButtonContent;
    private static readonly GUIContent deleteButtonContent;
    private static readonly GUIContent addButtonContent;

    static EditorList()
    {
        buttonWidth = GUILayout.Width(25);

        deleteButtonContent = new GUIContent("-", "Delete");
        addButtonContent = new GUIContent("+", "Add");
        moveButtonContent = new GUIContent("\u21b4", "Move down");
        duplicateButtonContent = new GUIContent("+", "Duplicate");
    }

    public static void Show(SerializedProperty list, EditorListOptions options = EditorListOptions.Default)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox($"{list.name} is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool showListLabel = (options & EditorListOptions.ShowLabel) != 0;
        bool showListSize = (options & EditorListOptions.ShowSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }

        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }

            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Multi-list editing of different sizes is not supported.", MessageType.Info);
            }

            ShowElements(list, options);
        }

        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static void ShowElements(SerializedProperty list, EditorListOptions options)
    {
        bool showElementLabels = (options & EditorListOptions.ShowElementLabels) != 0;
        bool showButtons = (options & EditorListOptions.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }

            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }

            if (!showButtons) continue;

            ShowButtons(list, i);
            EditorGUILayout.EndHorizontal();
        }

        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, buttonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }

        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, buttonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }

        if (!GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, buttonWidth)) return;

        int oldSizeOfList = list.arraySize;
        list.DeleteArrayElementAtIndex(index);
        if (list.arraySize == oldSizeOfList)
        {
            list.DeleteArrayElementAtIndex(index);
        }
    }
}